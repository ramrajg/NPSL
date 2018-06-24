using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Threading;
using System.Collections.Generic;
using NPSLCore.Models.DB;
using NPSL.Models.Models;

namespace ReconsileProcess
{
    class ReconclieProcessing
    {
        public bool KeepGoing { get; set; }

        public ReconclieProcessing()
        {
            KeepGoing = true;
        }

        public void ProcessFile()
        {
            while (KeepGoing)
            {
                List<ReconsileTemplate> ReconsileTemplateLst = DBContext.ExecuteTransactional<ReconsileTemplate>("P_GETRECONSILE_TEMPLATE");
                foreach (var item in ReconsileTemplateLst)
                {
                    string fileName = "";
                    try
                    {
                        string DateofProcessing = DateTime.Now.ToString("ddMMyyyy");
                        int NumberofColumns = 0;
                        string[] Sourcefiles = Directory.GetFiles(item.SourceFolder, "*" + item.SourceExtention, SearchOption.AllDirectories);
                        string[] Destinationfiles = Directory.GetFiles(item.DestinationFolder, "*" + item.DestinationExtention, SearchOption.AllDirectories);
                        fileName = Path.GetRandomFileName();
                        string path = item.SourceCompletionPath + "\\" + item.TemplateName + "\\" + DateofProcessing + "\\" + fileName + ".txt";
                        if (Sourcefiles.Length > 0)
                        {
                            CreateReconcileFile(Sourcefiles, path, item.SourceCompletionPath + "\\" + item.TemplateName + "\\" + DateofProcessing + "\\", item.SourceSubstringValue, item.SourceDelimiter, item.SourceHasHeader, out NumberofColumns);
                        }
                        if (Destinationfiles.Length > 0)
                        {
                            CreateReconcileFile(Destinationfiles, path, item.DestinationCompletionPath + "\\" + item.TemplateName + "\\" + DateofProcessing + "\\", item.DestinationSubstringValue, item.DestinationDelimiter, item.DestinationHasHeader, out NumberofColumns);
                        }
                        if (File.Exists(path))
                        {
                            Console.WriteLine("INSERTING INTO DB......." + path);
                            var param = new List<SqlParameter>
                            {
                                new SqlParameter("@FILEPATH", path),
                                new SqlParameter("@NUMBEROFCOLUMNS", NumberofColumns),
                                new SqlParameter("@TEMPLATEID", item.TemplateId),
                            };
                            var Data = DBContext.ExecuteTransactionalNonQuery("P_INSERTRECONSILEDATA", param);
                            File.Delete(path);

                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error : " + ex.Message.ToString());
                    }
                }
                Thread.Sleep(10000);
            }
        }

        static void CreateReconcileFile(string[] Files, string Filepath, string MoveFilepath, string SubstringValue, string Delimeter, bool? HasHeader, out int NumberofColumns)
        {
            string substring = "";
            string fileName = "";
            string[] strArr = SubstringValue.Split('|');
            NumberofColumns = strArr.Length;
            CreateDirectory(MoveFilepath);
            foreach (string dirFile in Files)
            {
                try
                {
                    const Int32 BufferSize = 128;
                    string ext = Path.GetExtension(dirFile).ToLower();
                    fileName = Path.GetFileNameWithoutExtension(dirFile).ToLower();
                    if (!File.Exists(Filepath))
                    {
                        File.Create(Filepath).Dispose();
                    }
                    if (!IsFileLocked(new FileInfo(dirFile)))
                    {
                        Console.WriteLine("READING " + fileName + " File");
                        using (var fileStream = File.OpenRead(dirFile))
                        using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
                        {
                            if (HasHeader == true) streamReader.ReadLine();
                            String line;
                            while ((line = streamReader.ReadLine()) != null)
                            {
                                if (line != "")
                                {
                                    substring = "";
                                    if (Delimeter == "SS")
                                    {
                                        for (int i = 0; i < strArr.Length; i++)
                                        {
                                            if (strArr[i] != "")
                                            {
                                                var numbers = strArr[i].Split(',').Select(Int32.Parse).ToList();
                                            substring = substring + line.Substring(numbers[0], numbers[1]) + ",";
                                            }
                                            else
                                            {
                                                substring = substring + ",";
                                            }
                                        }
                                    }
                                    else
                                    {
                                        var columnValue = line.Split(Delimeter).ToList();
                                        for (int i = 0; i < strArr.Length; i++)
                                        {
                                            if (strArr[i] != "")
                                            {
                                                substring = substring + columnValue[Int32.Parse(strArr[i])] + ",";
                                            }
                                            else
                                            {
                                                substring = substring + ",";
                                            }

                                        }
                                    }
                                    File.AppendAllText(Filepath, substring = substring.TrimEnd(',') + "," + fileName + "\n");
                                }
                            }
                        }
                        Console.WriteLine("MOVING " + fileName + " FILE.......");
                        if (!File.Exists(MoveFilepath + Path.GetFileName(dirFile)))
                        {
                            File.Move(dirFile, MoveFilepath + Path.GetFileName(dirFile));
                        }
                        else
                        {
                            File.Delete(dirFile);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("MOVING " + fileName + " FILE TO DIDNOTPROCESS (FILE READING)");
                    string DidNotProcessFolder = CreateDirectory(MoveFilepath + "DIDNOTPROCESS\\");
                    if (!File.Exists(DidNotProcessFolder + Path.GetFileName(dirFile)))
                    {
                        File.Move(dirFile, DidNotProcessFolder + Path.GetFileName(dirFile));
                    }
                    else
                    {
                        File.Delete(DidNotProcessFolder + Path.GetFileName(dirFile));
                        File.Move(dirFile, DidNotProcessFolder + Path.GetFileName(dirFile));
                    }
                   
                    throw new Exception(ex.Message.ToString());
                }
            }

        }
        static bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;
            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }
            return false;
        }
        static string CreateDirectory(string DirectoryName)
        {
            if (!Directory.Exists(DirectoryName))
            {
                Directory.CreateDirectory(DirectoryName);
            }
            return DirectoryName;
        }
    }
}

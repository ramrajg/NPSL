using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Threading;

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
                //string MoveFilepath = @"D:\\WorkStuff_Project\\RND\\MoveToComplete\\";
                //string FromFilepath = @"D:\\WorkStuff_Project\\RND\\FinalFiles\\";
                //string[] files = Directory.GetFiles(FromFilepath, "*.*", SearchOption.AllDirectories);
                //string fileName = Path.GetRandomFileName();
                //string path = @"D:\\WorkStuff_Project\\RND\\FileToInsert\\InsertRecord_" + fileName + ".txt";

                string Query = "select [Template_Name],[Source_Folder_Path] ,[Source_File_Extention] ,[Source_Completion_Path] ,[Source_Substring_Value] ,[Destination_Folder_Path],[Destination_File_Extention],[Destination_Completion_Path],[Destination_Substring_Value],[Destination_Delimiter],[Source_Delimiter] from Reconsile_Template";
                string oConnString = "Data Source=RAMRAJ;Initial Catalog=NPSL;Integrated Security=True";
                using (SqlConnection con = new SqlConnection(oConnString))
                {
                    using (SqlCommand cmd = new SqlCommand(Query, con))
                    {
                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            int NumberofColumns = 0;
                            string SourceMoveFilepath = reader["Source_Completion_Path"].ToString();
                            string SourceFromFilepath = reader["Source_Folder_Path"].ToString();
                            string[] Sourcefiles = Directory.GetFiles(SourceFromFilepath, "*" + reader["Source_File_Extention"].ToString(), SearchOption.AllDirectories);
                            string SourceSubstringValue = reader["Source_Substring_Value"].ToString();
                            string SourceDelimeter = reader["Source_Delimiter"].ToString();

                            string DestinationMoveFilepath = reader["Destination_Completion_Path"].ToString();
                            string DestinationFromFilepath = reader["Destination_Folder_Path"].ToString();
                            string[] Destinationfiles = Directory.GetFiles(DestinationFromFilepath, "*" + reader["Destination_File_Extention"].ToString(), SearchOption.AllDirectories);
                            string DestinationSubstringValue = reader["Destination_Substring_Value"].ToString();
                            string DestinationDelimeter = reader["Destination_Delimiter"].ToString();
                            string fileName = Path.GetRandomFileName();
                            string path = SourceMoveFilepath + fileName + ".txt";
                            if(Sourcefiles.Length > 0)
                            {
                                CreateReconcileFile(Sourcefiles, path, SourceMoveFilepath, SourceSubstringValue, SourceDelimeter,out  NumberofColumns);
                            }
                            if (Destinationfiles.Length > 0)
                            {
                                CreateReconcileFile(Destinationfiles, path, DestinationMoveFilepath, DestinationSubstringValue, DestinationDelimeter,out  NumberofColumns);
                            }
                            if (File.Exists(path))
                            {
                                Console.WriteLine("INSERTING INTO DB......." + path);
                                InsertToDB(path, NumberofColumns);
                                File.Delete(path);
                            }
                        }
                        con.Close();
                    }
                }
                Thread.Sleep(10000);
            }
        }

        static void CreateReconcileFile(string[] Files, string Filepath, string MoveFilepath, string SubstringValue, string Delimeter, out int NumberofColumns)
        {
            string substring = "";
            string[] strArr = SubstringValue.Split('|');
            NumberofColumns = strArr.Length;
            foreach (string dirFile in Files)
            {
                const Int32 BufferSize = 128;
                string ext = Path.GetExtension(dirFile).ToLower();
                if (!File.Exists(Filepath))
                {
                    File.Create(Filepath).Dispose();
                }
                if (!IsFileLocked(new FileInfo(dirFile)))
                {
                    Console.WriteLine("READING MKLP File");
                    using (var fileStream = File.OpenRead(dirFile))
                    using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
                    {
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
                                        var numbers = strArr[i].Split(',').Select(Int32.Parse).ToList();
                                        substring = substring + line.Substring(numbers[0], numbers[1]) + ",";
                                    }
                                }
                                else
                                {
                                    var columnValue = line.Split(Delimeter).ToList();
                                    for (int i = 0; i < strArr.Length; i++)
                                    {
                                        substring = substring + columnValue[Int32.Parse(strArr[i])] + ",";
                                    }
                                }
                                File.AppendAllText(Filepath, substring = substring.TrimEnd(',') + "\n");
                            }
                        }
                    }
                    Console.WriteLine("MOVING MKLP FILE.......");
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
        }
        static void InsertToDB(string path,int NumberofColumns)
        {
            string oConnString = "Data Source=RAMRAJ;Initial Catalog=NPSL;Integrated Security=True";
            using (SqlConnection con = new SqlConnection(oConnString))
            {
                using (SqlCommand cmd = new SqlCommand("P_INSERTRECONSILEDATA_New", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@FILEPATH", SqlDbType.VarChar).Value = path;
                    cmd.Parameters.Add("@NumberOfColumns", SqlDbType.Int).Value = NumberofColumns;
                    con.Open();
                    cmd.ExecuteNonQuery();
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
    }
}

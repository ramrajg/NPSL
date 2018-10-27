using NPSLCore.Models.DB;
using OfficeOpenXml;
using Spire.Xls;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace ReconsileProcess
{
    public class ReconclieProcessing
    {
        StreamWriter log;
        public bool KeepGoing { get; set; }
        public List<ReconsileTemplate> ReconsileTemplateLstCache;

        public ReconclieProcessing()
        {
            KeepGoing = true;
        }
        public void RefreshCacheList()
        {
            Log("Refresh Data");
            ReconsileTemplateLstCache = DBContext.ExecuteTransactional<ReconsileTemplate>("P_GETRECONSILE_TEMPLATE");
        }

        public void ProcessFile()
        {
            if (ReconsileTemplateLstCache == null)
            {
                RefreshCacheList();
            }
            while (KeepGoing)
            {
                foreach (var item in ReconsileTemplateLstCache)
                {
                    string fileName = "";
                    try
                    {
                        string DateofProcessing = DateTime.Now.ToString("ddMMyyyy");
                        int NumberofColumns = 0;
                        string[] Sourcefiles = Directory.GetFiles(item.SourceFolder, "*" + item.SourceExtention, SearchOption.AllDirectories);
                        fileName = Path.GetRandomFileName();
                        string path = item.SourceCompletionPath + "\\" + item.TemplateName + "\\" + DateofProcessing + "\\" + fileName + ".txt";
                        if (Sourcefiles.Length > 0)
                        {
                            CreateReconcileFile(Sourcefiles, path, item.SourceCompletionPath + "\\" + item.TemplateName + "\\" + DateofProcessing + "\\", item.SourceSubstringValue, item.SourceDelimiter, item.SourceHasHeader, out NumberofColumns);
                        }
                        if (File.Exists(path))
                        {
                            var param = new List<SqlParameter>
                            {
                                new SqlParameter("@FILEPATH", path),
                                new SqlParameter("@NUMBEROFCOLUMNS", NumberofColumns),
                                new SqlParameter("@TEMPLATEID", item.TemplateId),
                            };
                            var Data = DBContext.ExecuteTransactionalNonQuery("P_INSERTRECONSILEDATA", param);
                            File.Delete(path);
                            if (Environment.UserInteractive)
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("Sucessfully Reconsiled - Template Name " + item.TemplateName + " on : " + DateTime.Now);
                            }
                            else { Log("Sucessfully Reconsiled - Template Name " + item.TemplateName + " on : " + DateTime.Now); }
                        }
                    }
                    catch (Exception ex)
                    {
                        if (Environment.UserInteractive)
                        {
                            Console.BackgroundColor = ConsoleColor.Red;
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine("Error : " + ex.Message.ToString());
                        }
                        else { Log("Error : " + ex.Message.ToString()); }

                    }
                }
                Thread.Sleep(10000);
            }
        }

        public void CreateReconcileFile(string[] Files, string Filepath, string MoveFilepath, string SubstringValue, string Delimeter, bool? HasHeader, out int NumberofColumns)
        {
            string substring = "";
            string fileName = "";
            string fileNameWithExt = "";
            string[] strArr = SubstringValue.Split('|');
            NumberofColumns = strArr.Length;
            CreateDirectory(MoveFilepath);
            string[] formats = getSuppotedDateFormat();
            DateTime dateValue;
            foreach (string dirFile in Files)
            {
                try
                {
                    const Int32 BufferSize = 128;
                    string ext = Path.GetExtension(dirFile).ToLower();
                    fileName = Path.GetFileNameWithoutExtension(dirFile).ToLower();
                    fileNameWithExt = Path.GetFileName(dirFile).ToLower();
                    if (!File.Exists(Filepath))
                    {
                        File.Create(Filepath).Dispose();
                    }
                    if (!IsFileLocked(new FileInfo(dirFile)))
                    {
                        if (Environment.UserInteractive)
                        {
                            Console.ForegroundColor = ConsoleColor.Gray;
                            Console.WriteLine("READING " + fileName + " File");
                        }
                        else { Log("READING " + fileName + " File"); }
                        if (ext.ToLower() == ".xls" || ext.ToLower() == ".xlsx")
                        {
                            var xlsxFilePath = "";
                            if (ext.ToLower() == ".xls")
                            {
                                xlsxFilePath = ConvertXLS_XLSX(dirFile, MoveFilepath + Path.GetRandomFileName() + ".xlsx");
                            }
                            else
                            {
                                xlsxFilePath = dirFile;
                            }
                            using (ExcelPackage xlPackage = new ExcelPackage(new FileInfo(xlsxFilePath)))
                            {
                                var myWorksheet = xlPackage.Workbook.Worksheets.First(); //select sheet here
                                var totalRows = myWorksheet.Dimension.End.Row;
                                var totalColumns = myWorksheet.Dimension.End.Column;

                                var sb = new StringBuilder(); //this is your your data
                                for (int rowNum = Int32.Parse(Delimeter) + 1; rowNum <= totalRows; rowNum++) //selet starting row here
                                {
                                    substring = "";
                                    var row = myWorksheet.Cells[rowNum, 1, rowNum, totalColumns].Select(c => c.Value == null ? string.Empty : c.Value.ToString());
                                    sb.AppendLine(string.Join(",", row));
                                    for (int i = 0; i < strArr.Length; i++)
                                    {
                                        if (strArr[i] != "")
                                        {
                                            if (i == 2)
                                            {
                                                substring = substring + row.ElementAt(Int32.Parse(strArr[i])).Replace(".", "") + ",";
                                            }
                                            else if (i == 1)
                                            {
                                                if (row.ElementAt(Int32.Parse(strArr[i])) != "")
                                                {

                                                    DateTime.TryParseExact(row.ElementAt(Int32.Parse(strArr[i])), formats, System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue);
                                                    substring = substring + dateValue.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + ",";
                                                }
                                                else
                                                {
                                                    DateTime.TryParseExact(DateTime.Now.ToString(), formats, System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue);
                                                    substring = substring + dateValue.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + ",";
                                                }
                                            }
                                            else
                                            {
                                                if (row.ElementAt(Int32.Parse(strArr[i])) != "")
                                                {
                                                    substring = substring + row.ElementAt(Int32.Parse(strArr[i])) + ",";
                                                }
                                                else
                                                {
                                                    substring = substring + ",";
                                                }

                                            }
                                        }
                                        else
                                        {
                                            substring = substring + ",";
                                        }

                                    }
                                    File.AppendAllText(Filepath, substring = substring.TrimEnd(',') + "," + fileNameWithExt + "\n");
                                }
                            }
                            if (ext.ToLower() == ".xls")
                            {
                                if (File.Exists(xlsxFilePath))
                                {
                                    File.Delete(xlsxFilePath);
                                }
                            }
                        }
                        else
                        {
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
                                                    if (i == 2)
                                                    {
                                                        var numbers = strArr[i].Split(',').Select(Int32.Parse).ToList();
                                                        substring = substring + line.Substring(numbers[0] - 1, numbers[1]).Replace(".", "") + ",";
                                                    }
                                                    else if (i == 1)
                                                    {
                                                        var numbers = strArr[i].Split(',').Select(Int32.Parse).ToList();
                                                        if (line.Substring(numbers[0] - 1, numbers[1]) != "")
                                                        {
                                                            DateTime.TryParseExact(line.Substring(numbers[0] - 1, numbers[1]), formats, System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue);
                                                            substring = substring + dateValue.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + ",";
                                                        }
                                                        else
                                                        {
                                                            DateTime.TryParseExact(DateTime.Now.ToString(), formats, System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue);
                                                            substring = substring + dateValue.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + ",";
                                                        }

                                                    }
                                                    else
                                                    {
                                                        var numbers = strArr[i].Split(',').Select(Int32.Parse).ToList();
                                                        if (line.Substring(numbers[0] - 1, numbers[1]) != "")
                                                        {
                                                            substring = substring + line.Substring(numbers[0] - 1, numbers[1]) + ",";
                                                        }
                                                        else { substring = substring + ","; }
                                                    }
                                                }
                                                else
                                                {
                                                    substring = substring + ",";
                                                }
                                            }
                                        }
                                        else
                                        {
                                            var columnValue = line.Split(new string[] { Delimeter }, StringSplitOptions.None).ToList();
                                            for (int i = 0; i < strArr.Length; i++)
                                            {
                                                if (strArr[i] != "")
                                                {
                                                    if (i == 2)
                                                    {
                                                        substring = substring + columnValue[Int32.Parse(strArr[i])].Replace(".", "") + ",";
                                                    }
                                                    else if (i == 1)
                                                    {
                                                        if (columnValue[Int32.Parse(strArr[i])] != "")
                                                        {
                                                            DateTime.TryParseExact(columnValue[Int32.Parse(strArr[i])], formats, System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue);
                                                            substring = substring + dateValue.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + ",";
                                                        }
                                                        else
                                                        {
                                                            DateTime.TryParseExact(DateTime.Now.ToString(), formats, System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue);
                                                            substring = substring + dateValue.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + ",";
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (columnValue[Int32.Parse(strArr[i])] != "")
                                                        {
                                                            substring = substring + columnValue[Int32.Parse(strArr[i])] + ",";
                                                        }
                                                        else { substring = substring + ","; }
                                                    }
                                                }
                                                else
                                                {
                                                    substring = substring + ",";
                                                }

                                            }
                                        }
                                        File.AppendAllText(Filepath, substring = substring.TrimEnd(',') + "," + fileNameWithExt + "\n");
                                    }
                                }
                            }

                        }
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
        public static string ConvertXLS_XLSX(string file,string xlsxPath)
        {
            Workbook workbook = new Workbook();
            workbook.LoadFromFile(file);
            workbook.SaveToFile(xlsxPath, ExcelVersion.Version2013);

            return xlsxPath;
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
        private void Log(string strText)
        {
            if (Environment.UserInteractive)
            {
                Console.WriteLine(strText);
            }
            else
            {
                if (!File.Exists("logfile.txt"))
                {
                    log = new StreamWriter("logfile.txt");
                }
                else
                {
                    log = File.AppendText("logfile.txt");
                }
                log.WriteLine(DateTime.Now);
                log.WriteLine(strText);
                log.WriteLine();
                log.Close();
            }
        }
        private string[] getSuppotedDateFormat()
        {
            string[] DateFormateList =  { "M/d/yyyy h:mm:ss tt", "M/d/yyyy h:mm tt",
                     "MM/dd/yyyy hh:mm:ss", "M/d/yyyy h:mm:ss",
                     "M/d/yyyy hh:mm tt", "M/d/yyyy hh tt",
                     "M/d/yyyy h:mm", "M/d/yyyy h:mm",
                     "MM/dd/yyyy hh:mm", "M/dd/yyyy hh:mm","yyyyMMdd","dd-MM-yyyy hh:mm:ss","dd-MM-yyyy HH:mm: ss"};
            try
            {
                DateFormateList = File.ReadAllLines("DateFormatSupported.txt");
            }
            catch (Exception ex)
            {
                return DateFormateList;
            }
            return DateFormateList;
        }
    }
}

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
                string MoveFilepath = @"D:\\WorkStuff_Project\\RND\\MoveToComplete\\";
                string FromFilepath = @"D:\\WorkStuff_Project\\RND\\FinalFiles\\";
                string[] files = Directory.GetFiles(FromFilepath, "*.*", SearchOption.AllDirectories);
                string fileName = Path.GetRandomFileName();
                string path = @"D:\\WorkStuff_Project\\RND\\FileToInsert\\InsertRecord_" + fileName + ".txt";


                //string MoveFilepath = @"F:\\Sachine_Prj\\MovePath\\MoveToComplete\\";
                //string FromFilepath = @"F:\\Sachine_Prj\\FinalFiles\\";
                //string[] files = Directory.GetFiles(FromFilepath, "*.*", SearchOption.AllDirectories);
                //string fileName = Path.GetRandomFileName();
                //string path = @"F:\\Sachine_Prj\\MovePath\\FileToInsert\\InsertRecord_" + fileName + ".txt";


                string RRN;
                string Date;
                string Amount;

                foreach (string dirFile in files)
                {
                    const Int32 BufferSize = 128;
                    string ext = Path.GetExtension(dirFile).ToLower();
                    if (!File.Exists(path))
                    {
                        File.Create(path).Dispose();
                    }
                    if (ext == ".mklp")
                    {
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
                                        RRN = line.Substring(9, 12);
                                        Date = line.Substring(157, 6);
                                        Amount = line.Substring(166, 15);
                                        File.AppendAllText(path, RRN + "," + Date + "," + Amount + "\n");
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
                    else if (ext == ".rc")
                    {
                        if (!IsFileLocked(new FileInfo(dirFile)))
                        {
                            Console.WriteLine("READING RC File");
                            using (var fileStream = File.OpenRead(dirFile))
                            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
                            {
                                String line;
                                while ((line = streamReader.ReadLine()) != null)
                                {
                                    if (line != "")
                                    {
                                        RRN = line.Substring(25, 12);
                                        Date = line.Substring(129, 6);
                                        Amount = line.Substring(114, 15);
                                        File.AppendAllText(path, RRN + "," + Date + "," + Amount + "\n");
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
                if (File.Exists(path))
                {
                    Console.WriteLine("INSERTING INTO DB......." + path);
                    //InsertToDB(path);
                    //File.Delete(path);
                }
                Thread.Sleep(10000);
            }
        }
        static void InsertToDB(string path)
        {
            string oConnString = "Data Source=RAMRAJ;Initial Catalog=NPSL;Integrated Security=True";
            using (SqlConnection con = new SqlConnection(oConnString))
            {
                using (SqlCommand cmd = new SqlCommand("P_INSERTRECONSILEDATA", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@FILEPATH", SqlDbType.VarChar).Value = path;
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        private bool IsFileLocked(FileInfo file)
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

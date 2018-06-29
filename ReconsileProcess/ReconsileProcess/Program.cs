using ServiceBrokerListener.Domain;
using System;
using System.Configuration;
using System.Threading;


namespace ReconsileProcess
{
    class Program
    {
        static void Main(string[] args)
        {
            var listener = new SqlDependencyEx(ConfigurationManager.ConnectionStrings["ReconsileConnection"].ConnectionString, ConfigurationManager.AppSettings["DatabaeName"], ConfigurationManager.AppSettings["TableName"]);
            ReconclieProcessing processing = new ReconclieProcessing();
            listener.TableChanged += (o, e) => processing.RefreshCacheList();
            listener.Start();
            Thread t = new Thread(processing.ProcessFile);
            t.IsBackground = true;
            t.Start();
            while (true)
            {
                var keyInfo = Console.ReadKey();
                if (keyInfo.Key == ConsoleKey.C && keyInfo.Modifiers == ConsoleModifiers.Control)
                {
                    processing.KeepGoing = false;
                    listener.Stop();
                    break;
                }
            }
            t.Join();




            //ReconclieProcessing processing = new ReconclieProcessing();
            //Thread t = new Thread(processing.ProcessFile);
            //t.IsBackground = true;
            //t.Start();

            //while (true)
            //{
            //    var keyInfo = Console.ReadKey();
            //    if (keyInfo.Key == ConsoleKey.C && keyInfo.Modifiers == ConsoleModifiers.Control)
            //    {
            //        processing.KeepGoing = false;
            //        break;
            //    }
            //}
            //t.Join();
        }
        

    }

}



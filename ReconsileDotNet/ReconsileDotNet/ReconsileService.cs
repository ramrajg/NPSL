using ReconsileProcess;
using ServiceBrokerListener.Domain;
using System;
using System.Configuration;
using System.Threading;

namespace ReconsileDotNet
{
    public class ReconsileService
    {
        SqlDependencyEx listener = new SqlDependencyEx(ConfigurationManager.ConnectionStrings["ReconsileConnection"].ConnectionString, ConfigurationManager.AppSettings["DatabaseName"], ConfigurationManager.AppSettings["TableName"]);
        ReconclieProcessing processing = new ReconclieProcessing();
        public void Start()
        {
            listener.TableChanged += (o, e) => processing.RefreshCacheList();
            listener.Start();
            Thread t = new Thread(processing.ProcessFile);
            t.IsBackground = true;
            t.Start();
            //    //while (true)
            //    //{
            //    //    var keyInfo = Console.ReadKey();
            //    //    if (keyInfo.Key == ConsoleKey.C && keyInfo.Modifiers == ConsoleModifiers.Control)
            //    //    {
            //    //        processing.KeepGoing = false;
            //    //        listener.Stop();
            //    //        break;
            //    //    }
            //    //}
            t.Join();
        }
        public void Stop()
        {
            processing.KeepGoing = false;
            listener.Stop();
            // write code here that runs when the Windows Service stops.  
        }
    }
}

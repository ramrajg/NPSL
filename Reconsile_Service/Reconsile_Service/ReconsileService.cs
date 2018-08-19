using ReconsileProcess;
using ServiceBrokerListener.Domain;
using System.Configuration;
using System.Threading;

namespace Reconsile_Service
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
            //t.Join();
        }
        public void Stop()
        {
            processing.KeepGoing = false;
            listener.Stop();
        }
    }
}

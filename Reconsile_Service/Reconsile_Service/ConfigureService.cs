using Topshelf;

namespace Reconsile_Service
{
    internal static class ConfigureService
    {
        internal static void Configure()
        {
            HostFactory.Run(configure =>
            {
                configure.Service<ReconsileService>(service =>
                {
                    service.ConstructUsing(s => new ReconsileService());
                    service.WhenStarted(s => s.Start());
                    service.WhenStopped(s => s.Stop());
                });
                //Setup Account that window service use to run.  
                configure.RunAsLocalSystem();
                configure.SetServiceName("ReconsileService");
                configure.SetDisplayName("ReconsileService");
                configure.SetDescription("Reconsile Process Service to Process Bank File");
            });
        }
    }
}

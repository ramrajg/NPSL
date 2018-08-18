using Topshelf;

namespace ReconsileDotNet
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
                configure.SetServiceName("ReconsileProcess");
                configure.SetDisplayName("ReconsileProcess");
                configure.SetDescription("ReconsileProcess to Reconsile the Bank Files");
            });
        }
    }
}

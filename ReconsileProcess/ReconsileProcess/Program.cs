using Microsoft.Extensions.Configuration;
using System;
using System.Configuration;
using System.IO;
using System.Threading;


namespace ReconsileProcess
{
    class Program
    {
        static void Main(string[] args)
        {
            //var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
            //     .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            //IConfigurationRoot configuration = builder.Build();

            //Console.WriteLine(ConfigurationManager.AppSettings["test"]);
            //Console.WriteLine(ConfigurationManager.ConnectionStrings["CharityManagement"].ConnectionString);
            //Console.ReadLine();

            ReconclieProcessing processing = new ReconclieProcessing();
            Thread t = new Thread(processing.ProcessFile);
            t.IsBackground = true;
            t.Start();

            while (true)
            {
                var keyInfo = Console.ReadKey();
                if (keyInfo.Key == ConsoleKey.C && keyInfo.Modifiers == ConsoleModifiers.Control)
                {
                    processing.KeepGoing = false;
                    break;
                }
            }
            t.Join();
        }

    }
    
}



using System;
using System.Threading;

namespace ReconsileProcess
{
    class Program
    {
        static void Main(string[] args)
        {
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



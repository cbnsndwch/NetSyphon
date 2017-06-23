using System;
using System.Diagnostics;
using NetSyphon.Cli;
using NetSyphon.Commands.Contracts;
using NetSyphon.Commands.Implementations;
using NetSyphon.IoC;
using NetSyphon.Models;
using PowerArgs;

namespace NetSyphon
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                // register available commands
                RegisterCommands();

                //args = new[] { "-h" };
                args = new[] { "job", "D:\\test.json" };
                //args = new[] { "new", "D:\\test.json", "D:\\out.json" };

                Args.InvokeAction<Entry>(args);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception ocurred: {e.Message}");
                if (e is AggregateException)
                {
                    var level = 0;
                    while (e.InnerException != null)
                    {
                        var inner = e.InnerException;
                        Console.WriteLine($"Inner Exception {++level} message: {inner.Message}");
                    }
                }

                //Console.WriteLine($"StackTrace: {e.StackTrace}");
            }

#if DEBUG

            if (Debugger.IsAttached)
            {
                // keep the Console window open if in Debug mode and the debugger is attached
                Console.ReadLine();
            }
#endif
        }

        /// <summary>
        /// Register available commands with the Service Registry
        /// </summary>
        private static void RegisterCommands()
        {
            // register the RunJobCommand implementation
            ServiceLocator.Instance.RegisterService<ICommand<JobDescription>, RunJobCommand>(nameof(Entry.RunJob));
        }
    }
}

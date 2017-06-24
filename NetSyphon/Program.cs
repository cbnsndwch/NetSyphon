using System;
using System.Diagnostics;
using System.IO;
using MongoDB.Bson;
using NetSyphon.Cli;
using NetSyphon.Commands.Contracts;
using NetSyphon.Commands.Implementations;
using NetSyphon.IoC;
using NetSyphon.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
                args = new[] { "job", "D:\\out.json" };
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

        //private static void Foo()
        //{
        //    var fileIn = "D:\\JobDescription.json";
        //    var fileOut = "D:\\out.json";
        //    var json = File.ReadAllText(fileIn);
        //    var x = JsonConvert.DeserializeObject<JobDescription>(json);

        //    x.Sections.Add(new JobSection
        //    {
        //        Name = "Producto",
        //        Sql = "SELECT * FROM Producto",
        //        Template = JsonConvert.DeserializeObject<ExpandoObject>(
        //            @"{
        //                ""_id"": ""$Id"",
        //                ""Nombre"": ""$Nombre"",
        //                ""Descripcion"": ""$Descripcion"",
        //                ""CategoriaProducto"": ""@CategoriaProducto"",
        //                ""PrecioUnitario"": ""$PrecioUnitario"",
        //                ""PermitirVenta"": ""$PermitirVenta"",
        //            }")
        //    });

        //    json = JsonConvert.SerializeObject(x, Formatting.Indented);
        //    File.WriteAllText(fileOut, json);
        //    Environment.Exit(0);
        //}

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

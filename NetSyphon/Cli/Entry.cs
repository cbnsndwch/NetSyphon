using System;
using System.IO;
using log4net;
using NetSyphon.Commands.Contracts;
using NetSyphon.IoC;
using NetSyphon.Models;
using Newtonsoft.Json;
using PowerArgs;

namespace NetSyphon.Cli
{
    public class Entry
    {
        #region Fields

        private ILog _log = LogManager.GetLogger(typeof(Entry));

        #endregion

        #region Global Options

        [HelpHook, ArgShortcut("h"), ArgDescription("Displays this help information")]
        public bool Help { get; set; }

        #endregion

        #region ActionMethods (Commands)

        [ArgActionMethod, ArgShortcut("job"), ArgDescription("Runs a job specified in a config file")]
        public void RunJob(RunJobCliArgs cliArgs)
        {
            // TODO: Simple debug output. Use log4net with a ConsoleAppender here.
            Console.WriteLine($"Config file is: {cliArgs.ConfigFile}");

            if (!File.Exists(cliArgs.ConfigFile))
                throw new ArgException($"The specified Job Cofiguration file [{cliArgs.ConfigFile}] does not exist.");

            JobDescription model;
            try
            {
                var jsonText = File.ReadAllText(cliArgs.ConfigFile);
                model = JsonConvert.DeserializeObject<JobDescription>(jsonText);
            }
            catch (Exception e)
            {
                throw new AggregateException("An error occurred while loading the Job Configuration file. See InnerException for details", e);
            }

            // TODO: Add JsonSchema validation of the JobDescription

            // resolve the command
            ICommand<JobDescription> command;
            try
            {
                command = ServiceLocator.Instance.Get<ICommand<JobDescription>>(nameof(RunJob));
            }
            catch (Exception e)
            {
                throw new AggregateException($"An error occurred while resolving the {nameof(RunJob)} command. See InnerException for details", e);
            }

            // execute the command
            try
            {
                command.Execute(model);
            }
            catch (Exception e)
            {
                throw new AggregateException($"An error occurred while executing the {nameof(RunJob)} command. See InnerException for details", e);
            }
        }

        [ArgActionMethod, ArgShortcut("new"), ArgDescription("Generates a new Job Configuration file from the specified database")]
        public void GenerateConfigFile(GenerateConfigFileCliArgs cliArgs)
        {
            Console.WriteLine($"Config file source is: {cliArgs.ConfigFile}");
            Console.WriteLine($"Config file destination is: {cliArgs.OutputFile}");
        }

        #endregion

        #region Entry Point

        public void Main() { }

        #endregion
    }
}

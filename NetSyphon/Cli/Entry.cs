using System;
using log4net;
using log4net.Core;
using PowerArgs;

namespace NetSyphon
{
    public class Entry
    {
        #region Fields

        private ILog _log = LogManager.GetLogger(typeof(Entry));

        #endregion

        #region Options

        [HelpHook, ArgShortcut("h"), ArgDescription("Displays this help information")]
        public bool Help { get; set; }

        #endregion

        #region ActionMethods (Commands)

        [ArgActionMethod, ArgShortcut("job"), ArgDescription("Runs a job specified in a config file")]
        public void RunJob(RunJobCliArgs cliArgs)
        {
            Console.WriteLine($"Config file is: {cliArgs.ConfigFile}");
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

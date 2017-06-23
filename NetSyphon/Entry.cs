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
        public void RunJob(RunJobArgs args)
        {
            Console.WriteLine($"Config file is: {args.ConfigFile}");
        }

        [ArgActionMethod, ArgShortcut("new"), ArgDescription("Generates a new Job Configuration file from the specified database")]
        public void GenerateConfigFile(GenerateConfigFileArgs args)
        {
            Console.WriteLine($"Config file source is: {args.ConfigFile}");
            Console.WriteLine($"Config file destination is: {args.OutputFile}");
        }

        #endregion

        #region Entry Point

        public void Main() { }

        #endregion
    }

    /// <summary>
    /// Wraps the CLI arguments for the RunJob action
    /// </summary>
    public class RunJobArgs
    {
        /// <summary>
        /// The path to the Job Configuration file describing the job
        /// </summary>
        [ArgRequired, ArgPosition(1), ArgShortcut("c"), ArgDescription("Full path to the Job Configuration file")]
        public string ConfigFile { get; set; }
    }

    /// <summary>
    /// Wraps the CLI arguments for the GenerateConfigFile action
    /// </summary>
    public class GenerateConfigFileArgs 
    {
        /// <summary>
        /// The path to the Job Configuration file containing the RDBMS DB connection info
        /// </summary>
        [ArgRequired, ArgPosition(1), ArgShortcut("c"), ArgDescription("Full path to the Job Configuration file contining the RDBMS DB connection info")]
        public string ConfigFile { get; set; }

        /// <summary>
        /// The path to write the generated Job Configuration file out to
        /// </summary>
        [ArgRequired, ArgPosition(2), ArgShortcut("o"), ArgDescription("Output path for the generated Job Configuration file")]
        public string OutputFile { get; set; }
    }
}

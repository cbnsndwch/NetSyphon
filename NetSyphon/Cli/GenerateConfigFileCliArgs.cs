using PowerArgs;

namespace NetSyphon.Cli
{
    /// <summary>
    /// Wraps the CLI arguments for the GenerateConfigFile action
    /// </summary>
    public class GenerateConfigFileCliArgs
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
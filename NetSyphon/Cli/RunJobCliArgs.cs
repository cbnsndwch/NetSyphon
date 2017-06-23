using PowerArgs;

namespace NetSyphon.Cli
{
    /// <summary>
    /// Wraps the CLI arguments for the RunJob action
    /// </summary>
    public class RunJobCliArgs
    {
        /// <summary>
        /// The path to the Job Configuration file describing the job
        /// </summary>
        [ArgRequired, ArgPosition(1), ArgShortcut("c"), ArgDescription("Full path to the Job Configuration file")]
        public string ConfigFile { get; set; }
    }
}
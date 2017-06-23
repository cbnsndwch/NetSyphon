using System;
using NetSyphon.Commands.Contracts;
using NetSyphon.Models;

namespace NetSyphon.Commands.Implementations
{
    /// <summary>
    /// A command to run and ETL job, as specified by the Job Configuration model
    /// </summary>
    public class RunJobCommand : ICommand<RunJobModel>
    {
        /// <summary>
        /// The entry point to the command
        /// </summary>
        /// <param name="model">An instance of <see cref="RunJobModel"/> describing the ETL job</param>
        public void Execute(RunJobModel model)
        {
            if (model==null)
                throw new ArgumentException("The Job Description model cannot be null");
        }
    }
}

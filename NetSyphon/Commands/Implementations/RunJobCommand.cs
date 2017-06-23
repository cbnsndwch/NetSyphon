using System;
using NetSyphon.Commands.Contracts;
using NetSyphon.Models;

namespace NetSyphon.Commands.Implementations
{
    /// <summary>
    /// A command to run and ETL job, as specified by the Job Configuration model
    /// </summary>
    public class RunJobCommand : ICommand<JobDescription>
    {
        /// <summary>
        /// The entry point to the command
        /// </summary>
        /// <param name="model">An instance of <see cref="JobDescription"/> describing the ETL job</param>
        public void Execute(JobDescription model)
        {
            if (model == null)
                throw new ArgumentException("The Job Description model cannot be null");



        }
    }
}

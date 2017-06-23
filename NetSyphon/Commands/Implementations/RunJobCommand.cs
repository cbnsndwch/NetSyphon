using System;
using System.Threading.Tasks;
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
        /// The entry point to the command for synchronous execution
        /// </summary>
        /// <param name="model">An instance of <see cref="JobDescription"/> describing the ETL job</param>
        public void Execute(JobDescription model)
        {
            if (model == null)
                throw new ArgumentException("The Job Description model cannot be null");



        }

        /// <summary>
        /// The entry point to the command for asynchronous execution
        /// </summary>
        /// <param name="model">An instance of <see cref="JobDescription"/> describing the ETL job</param>
        public Task ExecuteAsync(JobDescription model)
        {
            throw new NotImplementedException();
        }
    }
}

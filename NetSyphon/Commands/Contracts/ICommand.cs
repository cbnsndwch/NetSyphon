using System.Threading.Tasks;

namespace NetSyphon.Commands.Contracts
{
    /// <summary>
    /// A contract for an App Command that takes a model as parameter
    /// </summary>
    /// <typeparam name="TModel">The type of the command model data</typeparam>
    public interface ICommand<in TModel>
    {
        /// <summary>
        /// Synchronously executes the command with the specified data
        /// </summary>
        /// <param name="model">The data for the command</param>
        void Execute(TModel model);

        /// <summary>
        /// Asynchronously executes the command with the specified data
        /// </summary>
        /// <param name="model">The data for the command</param>
        /// <returns>A task representing the promise of the asynchronous execution of the command</returns>
        Task ExecuteAsync(TModel model);
    }
}

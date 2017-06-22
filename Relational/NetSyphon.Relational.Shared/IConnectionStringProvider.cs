namespace NetSyphon.Relational.Shared
{
    /// <summary>
    /// Interface for specifying ado.net provider name and connection string. Used to create custom connection string/ado.net factory name providers 
    /// for sources other than .config files, e.g. with usage in ASPNET5
    /// </summary>
    public interface IConnectionStringProvider
    {
        /// <summary>
        /// Gets the name of the provider which is the name of the DbProviderFactory specified in the connection string stored under the name specified.
        /// </summary>
        /// <param name="connectionStringName">Name of the connection string.</param>
        /// <returns></returns>
        string GetProviderName(string connectionStringName);

        /// <summary>
        /// Gets the connection string stored under the name specified
        /// </summary>
        /// <param name="connectionStringName">Name of the connection string.</param>
        /// <returns></returns>
        string GetConnectionString(string connectionStringName);
    }
}
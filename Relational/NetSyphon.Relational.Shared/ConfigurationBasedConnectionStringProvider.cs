using System.Configuration;

namespace NetSyphon.Relational.Shared
{
    /// <summary>
    /// Default implementation of IConnectionStringProvider which uses config files for its source.
    /// </summary>
    /// <seealso cref="IConnectionStringProvider" />
    public class ConfigurationBasedConnectionStringProvider : IConnectionStringProvider
    {
        /// <summary>
        /// Gets the name of the provider which is the name of the DbProviderFactory specified in the connection string stored under the name specified.
        /// </summary>
        /// <param name="connectionStringName">Name of the connection string.</param>
        /// <returns></returns>
        public string GetProviderName(string connectionStringName)
        {
            var providerName = ConfigurationManager.ConnectionStrings[connectionStringName].ProviderName;
            return !string.IsNullOrWhiteSpace(providerName) ? providerName : null;
        }

        /// <summary>
        /// Gets the connection string stored under the name specified
        /// </summary>
        /// <param name="connectionStringName">Name of the connection string.</param>
        /// <returns></returns>
        public string GetConnectionString(string connectionStringName)
        {
            return ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
        }
    }
}
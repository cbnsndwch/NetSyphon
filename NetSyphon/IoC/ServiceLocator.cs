using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;

namespace NetSyphon.IoC
{
    /// <summary>
    /// A service registry to support IoC via Service Location.
    /// TODO: Add support for other IoC frameworks
    /// </summary>
    public class ServiceLocator
    {
        #region Constructor

        /// <summary>
        /// Private constructor to simulate singleton-like dynamics
        /// </summary>
        private ServiceLocator() { }

        #endregion

        #region Properties

        /// <summary>
        /// A singleton-like point of access to the ServiceLocator
        /// </summary>
        public static ServiceLocator Instance = new ServiceLocator();

        private UnityContainer Container { get; } = new UnityContainer();

        #endregion

        #region Public Methods

        /// <summary>
        /// Registers a Service with the service locator by specifying the singleton instance to be returned, 
        /// optionally setting a key on it to allow for disambuiguation.
        /// </summary>
        /// <typeparam name="TService">The type of the service to register</typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public TService Get<TService>(string key = null) where TService : class
        {
            TService service;
            try
            {
                // attempt to resolve the service from the underlying IoC container
                service = Container.Resolve<TService>(key);
            }
            catch (Exception e)
            {
                // if an exception occurs, wrap it in a custom exception and throw
                throw new Exception($"An error ocurred resolving the requested service {typeof(TService).Name}. See InnerException for details", e);
            }

            // if the container resolved the srevice to a null reference, throw an exception
            if (service == null)
                throw new ArgumentException($"The requested service {typeof(TService).Name} could not be resolved");

            // if everything went well, return the resolved service
            return service;
        }

        /// <summary>
        /// Registers a Service with the service locator by specifying the singleton instance to be returned, 
        /// optionally setting a key on it to allow for disambuiguation.
        /// </summary>
        /// <typeparam name="TService">The type of the service to register.</typeparam>
        /// <param name="key">An optional key to allow for service implementation disambuiguation.</param>
        /// <param name="instance">
        /// The service implementation singleton to return when an instance is requested with the same key. 
        /// If no key is specified, the instance will be returned by default.
        /// </param>
        /// <returns>Returns the ServiceLocator instance itself to allow fluent registration of services.</returns>
        public ServiceLocator RegisterService<TService>(string key, TService instance) where TService : class
        {
            // register the instance with the underlying IoC container
            Container.RegisterInstance<TService>(key, instance);

            // and return the ServiceLocator singleton to allow call chaining.
            return this;
        }

        /// <summary>
        /// Register a Service with the service locator by specifying the type of the implementation 
        /// to be used when instances of the srevice are requested. 
        /// Supports optionally setting a key on it to allow for disambuiguation.
        /// </summary>
        /// <typeparam name="TService">The type of the service contract to register</typeparam>
        /// <typeparam name="TServiceImpl">The type of the service implementation to register</typeparam>
        /// <param name="key">An optional key to allow for service implementation disambuiguation.</param>
        /// <returns>Returns the ServiceLocator instance itself to allow fluent registration of services.</returns>
        public ServiceLocator RegisterService<TService, TServiceImpl>(string key) 
            where TServiceImpl : TService 
            where TService : class
        {
            // register the type mapping with the underlying IoC container
            Container.RegisterType<TService, TServiceImpl>(key);

            // and return the ServiceLocator singleton to allow call chaining.
            return this;
        }

        #endregion
    }
}

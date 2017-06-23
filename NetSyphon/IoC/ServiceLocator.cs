using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSyphon.IoC
{
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

        #endregion

        #region Public Methods

        public TService Get<TService>(object key = null)
        {
            return default(TService);
        }

        #endregion
    }
}

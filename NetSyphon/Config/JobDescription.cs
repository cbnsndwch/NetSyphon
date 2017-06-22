using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSyphon.Config
{
    public class JobDescription
    {
        #region SQL Options

        /// <summary>
        /// RDBMS connection string
        /// </summary>
        public string DatabaseConnection { get; set; }

        /// <summary>
        /// Name of the ADO.NET Provider to use for this connection
        /// </summary>
        public string ProviderName { get; set; }

        #endregion

        #region MongoDB Options

        /// <summary>
        /// MongoDB connection string URL
        /// </summary>
        public string MongoConnection { get; set; }

        #endregion

        #region Job Description

        /// <summary>
        /// The name of the section to use as an entry point to the Job
        /// </summary>
        public string StartAt { get; set; }

        /// <summary>
        /// All sections that define the Job
        /// </summary>
        public List<JobSection> Sections { get; set; }

        #endregion
    }
}

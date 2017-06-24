using System.Collections.Generic;

namespace NetSyphon.Models
{
    /// <summary>
    /// Describes an ETL job to extract data from various sources, transform it and load it into a MongoDB server
    /// </summary>
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

        #region Batching & Threading

        /// <summary>
        /// The number of items to read from the source before issuing a bulk write command to MongoDB
        /// </summary>
        public int BatchSize { get; set; }

        /// <summary>
        /// The number of threads to run in parallel
        /// TODO: Figure out the best way to partition source data to allow parallel queries without overlapping
        /// </summary>
        public int ThreadCount { get; set; }

        #endregion

        #region MongoDB Options

        /// <summary>
        /// MongoDB connection string URL
        /// </summary>
        public string MongoConnection { get; set; }

        /// <summary>
        /// The MongoDB database to connect to
        /// </summary>
        public string MongoDatabase { get; set; }
        
        #endregion

        #region Job Sections

        /// <summary>
        /// The name of the section to use as an entry point to the Job
        /// </summary>
        public string StartAt { get; set; }

        /// <summary>
        /// All sections that define the Job
        /// </summary>
        public List<JobSection> Sections { get; set; } = new List<JobSection>();

        #endregion
    }
}

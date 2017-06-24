using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NetSyphon.Models
{
    /// <summary>
    /// Describes an ETL job to extract data from various sources, transform it and load it into a MongoDB server
    /// </summary>
    public class JobDescription
    {
        #region Job Options

        [JsonConverter(typeof(StringEnumConverter))]
        public JobMode JobMode { get; set; }

        #endregion

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

        /// <summary>
        /// The MongoDB collection to use as destination
        /// </summary>
        public string MongoCollection { get; set; }
        
        #endregion

        #region Job Sections

        /// <summary>
        /// The name of the section to use as an entry point to the Job
        /// </summary>
        public string StartAt { get; set; }

        /// <summary>
        /// Gets the Start section of this job by matching existing sections against the section name declared in <see cref="StartAt"/>
        /// </summary>
        public JobSection StartSection => Sections.FirstOrDefault(s => s.Name == StartAt);

        /// <summary>
        /// All sections that define the Job
        /// </summary>
        public List<JobSection> Sections { get; set; } = new List<JobSection>();

        #endregion
    }
}

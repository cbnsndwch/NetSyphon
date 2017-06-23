using Newtonsoft.Json.Linq;

namespace NetSyphon.Models
{
    /// <summary>
    /// Describes a seection of an ETL job to extract data from various sources, transform it and load it into a MongoDB server
    /// </summary>
    public class JobSection
    {
        /// <summary>
        /// The name of the section. Will be used to determine the entry point to the job and to include sub-sections as nested arrays or objects.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The template for the output Document. Supports copying fields from the source directly into document fields as well as creating nested objects and arrays.
        /// </summary>
        public JObject Template { get; set; }

        /// <summary>
        /// A SQL query describing which data to extract from an RDBMS source
        /// </summary>
        public string Sql { get; set; }

        /// <summary>
        /// Not currently used. Meant to support Merge/Reentrant jobs
        /// </summary>
        public string MergeOn { get; set; }
    }
}
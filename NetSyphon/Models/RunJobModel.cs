using NetSyphon.Config;

namespace NetSyphon.Models
{
    public class RunJobModel
    {
        /// <summary>
        /// Describes an ETL job to extract data from various sources, transform it and load it into a MongoDB server
        /// </summary>
        public JobDescription JobDescription { get; set; }
    }
}
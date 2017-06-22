using Newtonsoft.Json.Linq;

namespace NetSyphon.Config
{
    public class JobSection
    {
        public string Name { get; set; }
        public JObject Template { get; set; }
        public string Sql { get; set; }
        public string Mergeon { get; set; }
    }
}
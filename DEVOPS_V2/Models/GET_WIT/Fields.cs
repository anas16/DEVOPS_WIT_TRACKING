using Newtonsoft.Json;

namespace DEVOPS_V2.Models
{
    class Fields
    {
        [JsonProperty("System.State")]
        public string SystemState { get; set; }

        [JsonProperty("System.Title")]
        public string SystemTitle { get; set; }

        [JsonProperty("System.Description")]
        public string SystemDescription { get; set; }

        [JsonProperty("System.Boardcolumn")]
        public string SystemBoardcolumn { get; set; }

        [JsonProperty("System.CreatedDate")]
        public string SystemCreatedDate { get; set; }
    }
}

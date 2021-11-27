using System.Collections.Generic;
using Newtonsoft.Json;

namespace DEVOPS_V2.Models
{
    class ValueRoot
    {
        public int Count { get; set; }
        public List<Value> Value { get; set; }
    }
    class Value
    {
        public int Id { get; set; }
        public int Rev { get; set; }
        public Fields Fields { get; set; } = new Fields();
        public string Url { get; set; }

    }
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

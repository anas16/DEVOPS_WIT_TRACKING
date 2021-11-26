using System.Collections.Generic;

namespace DEVOPS_V2.Models
{
    public class GetStart
    {
        public class Start_Value
        {
            public string id { get; set; }
            public string name { get; set; }
            public string url { get; set; }
            public string description { get; set; }
            public string identityUrl { get; set; }
            public string projectName { get; set; }
            public string projectId { get; set; }
        }

        public class Start_Root
        {
            public int count { get; set; }
            public List<Start_Value> value { get; set; }
        }
    }
}

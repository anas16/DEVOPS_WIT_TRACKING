using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Teams - GET URI https://dev.azure.com/{org}/_apis/teams?api-version=6.0-preview.3

namespace DEVOPS_V2.Models
{
    class GetTeam
    {
        public class Team_Value
        {
            public string id { get; set; }
            public string name { get; set; }
            public string url { get; set; }
            public string description { get; set; }
            public string identityUrl { get; set; }
            public string projectName { get; set; }
            public string projectId { get; set; }
        }

        public class Team_Root
        {
            public List<Team_Value> value { get; set; }
            public int count { get; set; }
        }
    }
}

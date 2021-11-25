using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Project - List URI https://dev.azure.com/{org}/_apis/projects?api-version=6.0

namespace DEVOPS_V2.Models
{
    class GetProj
    {
        public class Project_Value
        {
            public string id { get; set; }
            public string name { get; set; }
            public string url { get; set; }
            public string state { get; set; }
            public int revision { get; set; }
            public string visibility { get; set; }
            public DateTime lastUpdateTime { get; set; }
        }

        public class Project_Root
        {
            public int count { get; set; }
            public List<Project_Value> value { get; set; }
        }
    }
}

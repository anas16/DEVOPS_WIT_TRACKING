using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVOPS_V2.Models
{
    public class Uri_var
    {
        public readonly string BaseUrl = "https://dev.azure.com";
        public readonly string Api_Ver = "api-version=6.0";

        public string PAT { get; set; }
        //public string PAT = "zkoadtck5epdguan2byd6k4f4ezppruhctx2xdjlro24vv5dyiqq";
        public string Organisasi { get; set; }
        //public string Organisasi = "muhamadhafizanas";
        public string Project { get; set; }
        //public string Project = "TEST1";
        public string Team { get; set; }
        //public string Team = "TEST1Team";
    }
}

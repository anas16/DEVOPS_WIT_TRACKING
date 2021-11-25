using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVOPS_V2.Models
{
    class WkQuery
    {
        public string QueryType { get; set; }
        public string QueryResultType { get; set; }
        public string AsOf { get; set; }
        public List<WkColumn> Columns { get; set; } = new List<WkColumn>();
        public List<WkItem> WorkItems { get; set; } = new List<WkItem>();
    }
}

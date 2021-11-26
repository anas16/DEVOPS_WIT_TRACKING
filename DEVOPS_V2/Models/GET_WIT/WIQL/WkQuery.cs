using System.Collections.Generic;

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

namespace DEVOPS_V2.Models
{
    class Value
    {
        public int Id { get; set; }
        public int Rev { get; set; }
        public Fields Fields { get; set; } = new Fields();
        public string Url { get; set; }

    }
}

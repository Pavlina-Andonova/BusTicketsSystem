namespace PandaTour.Models
{
    using System.Collections.Generic;

    public class Line
    {
        public int Id { get; set; }
        public string LineName { get; set; }
        public ICollection<Schedule> Schedule { get; set; }
        public ICollection<LineStopStation> LineStopStation { get; set; }
    }
}

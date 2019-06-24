namespace PandaTour.Models
{
    using System.Collections.Generic;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Schedule
    {
        public int Id { get; set; }
        public int LineId { get; set; }
        public int BusId { get; set; }

        [DataType(DataType.Time)]
        [Column(TypeName = "Time")]
        public TimeSpan DepartureTime { get; set; }

        public Line Line { get; set; }
        public Bus Bus { get; set; }
        public ICollection<TicketStop> TicketStops { get; set; }
    }
}

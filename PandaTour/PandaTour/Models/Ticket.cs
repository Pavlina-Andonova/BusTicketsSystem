namespace PandaTour.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Ticket
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ScheduleId { get; set; }
        public decimal Price { get; set; }
        public int SeatId { get; set; }
        public int DepartureStationOrder { get; set; }
        public int ArrivalStationOrder { get; set; }

        [DataType(DataType.Date)]
        [Column(TypeName = "Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ForDate { get; set; }

        public User User { get; set; }
        public Schedule Schedule { get; set; }
        public Seat Seat { get; set; }
        public ICollection<TicketStop> TicketStops { get; set; }
    }
}

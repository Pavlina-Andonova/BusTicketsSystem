namespace PandaTour.Models
{
    using System.Collections.Generic;
    public class Seat
    {
        public int Id { get; set; }
        public Bus Bus { get; set; }
        public string SeatNumber { get; set; }
        public ICollection<TicketStop> TicketStops { get; set; }
    }
}

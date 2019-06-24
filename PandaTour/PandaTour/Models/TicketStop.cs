namespace PandaTour.Models
{
    using System;
    public class TicketStop
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public int LineStopStationId { get; set; }
        public int ScheduleId { get; set; }
        public int SeatId { get; set; }
        public DateTime ForDate { get; set; }
        public Ticket Ticket { get; set; }
        public Schedule Schedule { get; set; }
        public Seat Seat { get; set; }
        public LineStopStation LineStopStation { get; set; }
    }
}

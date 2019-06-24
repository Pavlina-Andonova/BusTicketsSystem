namespace PandaTour.Models
{
    using System.Collections.Generic;

    public class Bus
    {
        public int Id { get; set; }
        public string RegistrationNumber { get; set; }
        public ICollection<Seat> Seats { get; set; }
        public ICollection<Schedule> Schedule { get; set; }
    }
}

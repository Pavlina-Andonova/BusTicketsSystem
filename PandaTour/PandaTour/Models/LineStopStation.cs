namespace PandaTour.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    public class LineStopStation
    {
        public int Id { get; set; }
        public int LineId { get; set; }
        public int StopId { get; set; }
        public int CityStationId { get; set; }
        public int Order { get; set; }
        public bool IsLastStop { get; set; }

        [DataType(DataType.Time)]
        [Column(TypeName = "Time")]
        public TimeSpan TimeToReachFromTheFirstLineStop { get; set; }

        [DataType(DataType.Currency)]
        public decimal PriceToGetThere { get; set; }

        public Line Line { get; set; }
        public Stop Stop { get; set; }
        public CityStation CityStation { get; set; }
        public ICollection<TicketStop> TicketStops { get; set; }
    }
}

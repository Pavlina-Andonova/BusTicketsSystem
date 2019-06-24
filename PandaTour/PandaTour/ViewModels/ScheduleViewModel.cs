namespace PandaTour.ViewModels
{
    using System;
    public class ScheduleViewModel
    {
        public int ScheduleId { get; set; }
        public int LineId { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public TimeSpan TimeOnSpot { get; set; }
        public int StationId { get; set; }
        public string StationName { get; set; }
        public int Order { get; set; }
        public decimal PriceToGetThere { get; set; }
    }
}

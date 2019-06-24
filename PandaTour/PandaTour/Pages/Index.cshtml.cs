namespace PandaTour.Pages
{
    using PandaTour.ViewModels;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using PandaTour.Repositories;
    using PandaTour.Data;
    using System.Collections.Generic;
    using PandaTour.Models;
    using System;
    using System.Linq;

    public class IndexModel : PageModel
    {
        private LineRepository _lineRepository;
        private TicketRepository _ticketRepository;

        public IndexModel(LineRepository lineRepository, TicketRepository ticketRepository)
        {
            _lineRepository = lineRepository;
            _ticketRepository = ticketRepository;
        }
        public List<SelectDestinationViewModel> Destinations { get; set; }
        public List<SelectDestinationViewModel> StationsForSelectedStartingPoint { get; set; }
        public List<ScheduleViewModel> ScheduleForSelectedStartEndPointAndDate { get; set; }
        public void OnGet()
        {
            //_lineRepository.GetTime();
            //var res = _lineRepository.GetAllLinesByStartEndLocation(1, 3);
            //foreach (KeyValuePair<int, List<LineStopStation>> kv in res)
            //{
            //    var lineId = kv.Key;
            //    var StartPosition = $"{kv.Value.First().Line.LineName} ({kv.Value.First().CityStation.StationName})";
            //    var LastPosition = $"{kv.Value.Last().Line.LineName} ({kv.Value.Last().CityStation.StationName})";
            //}
            //_lineRepository.GetAllSchedulesForStartEndPoint(3, 2, 4);

        }

        private bool SaveTicket(int lineId, int scheduleId, int startId, int endId, List<int> seats, DateTime date)
        {
            List<Ticket> tickets = new List<Ticket>();
            var ticketPrice = CalculateTicketPrice(lineId, startId, endId);

            for (var i = 0; i < seats.Count; i++)
            {
                var ticket = new Ticket
                {
                    Price = ticketPrice,
                    ScheduleId = scheduleId,
                    SeatId = seats[i],
                    UserId = 1, // Hardcoded
                    ForDate = date
                };
                tickets.Add(ticket);
            }

            var result = _ticketRepository.SaveTickets(tickets, startId, endId, lineId);

            return result;
        }

        private decimal CalculateTicketPrice(int lineId, int startId, int endId)
        {
            return _ticketRepository.GetPriceForARoute(lineId, startId, endId);
        }

        private List<Seat> GetFreeSeats (int lineId, int scheduleId, int startId, int endId, DateTime forDate)
        {
            List<Seat> takenSeats = _ticketRepository.GetOverlappedSeats(lineId, scheduleId, startId, endId, forDate);
            List<int> takenSeatIds = takenSeats.Select(s => s.Id).ToList();
            List<Seat> allSeats = _ticketRepository.GetAllSeatsBySchedule(scheduleId, forDate);

            var freeSeats = allSeats.Where(s => !takenSeatIds.Contains(s.Id)).Select(s => s).ToList();
            return freeSeats;
        }
    }
}
 
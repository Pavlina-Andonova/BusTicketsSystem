namespace PandaTour.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using PandaTour.Data;
    using PandaTour.Models;
    using PandaTour.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class TicketRepository
    {
        private PandaTourContext _context;

        public TicketRepository(PandaTourContext context)
        {
            _context = context;
        }

        public bool SaveTickets(List<Ticket> tickets, int startStationId, int endStationId, int lineId)
        {
            bool operationSuccessfull = true;
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var reservedSeats = GetOverlappedSeats(lineId, tickets[0].ScheduleId, startStationId, endStationId, tickets[0].ForDate);
                    var reservedSeatIds = reservedSeats.Select(rs => rs.Id).ToList();
                    var requestSeatIds = tickets.Select(t => t.SeatId).ToList();

                    bool hasOverlappingSeats = reservedSeatIds.Intersect(requestSeatIds).Any();
                    if (hasOverlappingSeats)
                    {
                        throw new Exception();
                    }

                    int startStationOrder = _context.LineStopStations
                        .First(l => l.LineId == lineId && l.StopId == startStationId).Order;

                    int endStationOrder = _context.LineStopStations
                        .First(l => l.LineId == lineId && l.StopId == endStationId).Order;

                    foreach (var ticket in tickets)
                    {
                        ticket.DepartureStationOrder = startStationOrder;
                        ticket.ArrivalStationOrder = endStationOrder;
                        _context.Tickets.Add(ticket);
                    }
                    _context.SaveChanges();
                    List<int> stopStaionIds = _context.LineStopStations
                        .Where(l => l.LineId == lineId && l.Order >= startStationOrder && l.Order <= endStationOrder)
                        .Select(l => l.Id)
                        .ToList();

                    // Since we primarly use this table to prevent concurrency problems
                    // It's better not to save the last stop since it could be used as a starting
                    // stop for another ticket which would violate the unique constraint
                    stopStaionIds.Remove(stopStaionIds.Max());

                    foreach (var ticket in tickets)
                    {
                        foreach (var stationId in stopStaionIds)
                        {
                            var ticketStop = new TicketStop
                            {
                                ScheduleId = tickets[0].ScheduleId,
                                LineStopStationId = stationId,
                                TicketId = ticket.Id,
                                ForDate = ticket.ForDate,
                                SeatId = ticket.SeatId
                            };
                            _context.TicketStops.Add(ticketStop);
                        }
                    }

                    _context.SaveChanges();
                }
                catch (Exception e)
                {
                    operationSuccessfull = false;
                }
                finally
                {
                    if (!operationSuccessfull)
                    {
                        transaction.Rollback();
                    }
                    else
                    {
                        transaction.Commit();
                    }
                }
            }

            return operationSuccessfull;
        }

        public decimal GetPriceForARoute(int lineId, int startId, int endId)
        {
            int startStationOrder = _context.LineStopStations
                .First(l => l.LineId == lineId && l.StopId == startId).Order;

            int endStationOrder = _context.LineStopStations
                .First(l => l.LineId == lineId && l.StopId == endId).Order;

            decimal price = _context.LineStopStations
                .Where(l => l.LineId == lineId && l.Order > startStationOrder && l.Order <= endStationOrder)
                .Sum(l => l.PriceToGetThere);

            return price;
        }

        public List<Seat> GetOverlappedSeats(int lineId, int scheduleId, int startId, int endId, DateTime forDate)
        {
            int startStationOrder = _context.LineStopStations
                .First(l => l.LineId == lineId && l.StopId == startId).Order;

            int endStationOrder = _context.LineStopStations
                .First(l => l.LineId == lineId && l.StopId == endId).Order;

            var takenSeats = _context.Tickets
                            .Where(t => t.ScheduleId == scheduleId && t.ForDate == forDate)
                            .Where(t => (startStationOrder > t.DepartureStationOrder && startStationOrder < t.ArrivalStationOrder) ||
                                    (endStationOrder > t.DepartureStationOrder && endStationOrder < t.ArrivalStationOrder)
                            )
                            .Select(t => t.Seat)
                            .ToList();



            return takenSeats;
        }

        public List<Seat> GetAllSeatsBySchedule(int scheduleId, DateTime forDate)
        {
            return _context.Seats
                .Where(s => s.Bus.Id == _context.Schedules.First(sc => sc.Id == scheduleId).BusId)
                .Select(s => s)
                .ToList();
        }
    }
}

namespace PandaTour.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using PandaTour.Data;
    using PandaTour.Models;
    using PandaTour.ViewModels;
    using System.Collections.Generic;
    using System.Linq;

    public class LineRepository
    {
        private PandaTourContext _context;

        public LineRepository(PandaTourContext context)
        {
            _context = context;
        }

        public List<SelectDestinationViewModel> GetAllStartingDesinations()
        {
            return _context.LineStopStations
                .Where(ls => !ls.IsLastStop)
                .GroupBy(ls => ls.StopId,
                    (key, group) =>
                        new SelectDestinationViewModel
                        {
                            StopId = key,
                            StopName = group.First().Stop.StopName,
                        })
                .ToList();
        }

        // TODO verify behaviour
        public List<SelectDestinationViewModel> GetAllArrivalLocationsByStationId(int stationId)
        {
            var locations = _context.LineStopStations
                .Where(l => 
                    _context.LineStopStations.Where(ls => ls.StopId == stationId).Select(ls => ls.LineId).Contains(l.LineId))
                .Where(l => 
                    l.Order > _context.LineStopStations.First(ls => ls.LineId == l.LineId && ls.StopId == stationId).Order)
                .Select(l => new SelectDestinationViewModel
                {
                    StopId = l.StopId,
                    StopName = l.Stop.StopName
                })
                .ToList();

            return locations;
        }

        public Dictionary<int, List<LineStopStation>> GetAllLinesByStartEndLocation(int startStationID, int endStationId)
        {
            var lines = _context.LineStopStations
                .Include(l => l.Line)
                .Include(c => c.CityStation).ToList()
            .GroupBy(g => g.LineId)
            .Where(g => g.Select(s => s.StopId).Contains(startStationID) && g.Select(s => s.StopId).Contains(endStationId))
            .Where(g => g.First(s => s.StopId == startStationID).Order < g.First(s => s.StopId == endStationId).Order)
            .ToDictionary(group => group.Key, group => group.OrderBy(g => g.Order).ToList());

            return lines;
        }

        public Dictionary<int, List<ScheduleViewModel>> GetAllSchedulesForStartEndPoint(int lineId, int startStationID, int endStationId)
        {
            var startingLocationOrder = _context.LineStopStations
                .First(l => l.StopId == startStationID && l.LineId == lineId).Order;

            var lastLocationOrder = _context.LineStopStations
                .First(l => l.StopId == endStationId && l.LineId == lineId).Order;

            var schedules = _context.Schedules
                .Join(_context.LineStopStations,
                    schedule => schedule.LineId,
                    lineStation => lineStation.LineId,
                    (schedule, lineStation) => new ScheduleViewModel
                    {
                        Order = lineStation.Order,
                        PriceToGetThere = lineStation.PriceToGetThere,
                        TimeOnSpot = (lineStation.TimeToReachFromTheFirstLineStop + schedule.DepartureTime),
                        StationName = lineStation.Stop.StopName,
                        StationId = lineStation.StopId,
                        ScheduleId = schedule.Id,
                        DepartureTime = schedule.DepartureTime,
                        LineId = lineStation.LineId
                    }
                )
                .Where(l => l.Order >= startingLocationOrder && l.Order <= lastLocationOrder && l.LineId == lineId)
                .GroupBy(svm => svm.ScheduleId)
                .ToDictionary(group => group.Key, group => group.OrderBy(g => g.Order).ToList());

            return schedules;
        }

        public System.TimeSpan GetTime()
        {
            var time = _context.Lines
                .Where(s => s.Id == 1).Select(t => new
                {
                    Start = t.Schedule.Where(s => s.LineId == t.Id).First().DepartureTime,
                    End = t.LineStopStation.Where(ls => ls.LineId == t.Id && ls.Order == 1).First().TimeToReachFromTheFirstLineStop
                })
                .ToList();

            var res = time[0].Start + time[0].End;
            return res;
        }
    }
}

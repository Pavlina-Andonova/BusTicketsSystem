namespace PandaTour.Models
{
    using System;
    using System.Linq;
    using PandaTour.Data;
    using PandaTour.Models;

    public static class DbInitializer
    {
        public static void Initialize(PandaTourContext context)
        {
            // context.Database.EnsureCreated();

            // Look for any Buses
            if (context.Buses.Any())
            {
                return; // DB has been seeded
            }

            var buses = new Bus[] 
            {
                new Bus { RegistrationNumber = "CB 2711 TT" },
                new Bus { RegistrationNumber = "PB 6364 PP" },
                new Bus { RegistrationNumber = "PA 2039 XA" }
            };

            foreach (var bus in buses)
            {
                context.Buses.Add(bus);
            }
            context.SaveChanges();

            var sfBus = context.Buses.First(b => b.RegistrationNumber == "CB 2711 TT");
            var pldBus = context.Buses.First(b => b.RegistrationNumber == "PB 6364 PP");
            var pzBus = context.Buses.First(b => b.RegistrationNumber == "PA 2039 XA");

            var seats = new Seat[]
            {
                new Seat { SeatNumber = "A1", Bus = sfBus },
                new Seat { SeatNumber = "A2", Bus = sfBus },
                new Seat { SeatNumber = "A3", Bus = sfBus },
                new Seat { SeatNumber = "A4", Bus = sfBus },
                new Seat { SeatNumber = "A5", Bus = sfBus },
                new Seat { SeatNumber = "A6", Bus = sfBus },
                new Seat { SeatNumber = "A7", Bus = sfBus },
                new Seat { SeatNumber = "A8", Bus = sfBus },
                new Seat { SeatNumber = "A9", Bus = sfBus },
                new Seat { SeatNumber = "A10", Bus = sfBus },
                new Seat { SeatNumber = "A11", Bus = sfBus },
                new Seat { SeatNumber = "A12", Bus = sfBus },
                new Seat { SeatNumber = "A1", Bus = pldBus },
                new Seat { SeatNumber = "A2", Bus = pldBus },
                new Seat { SeatNumber = "A3", Bus = pldBus },
                new Seat { SeatNumber = "A4", Bus = pldBus },
                new Seat { SeatNumber = "A5", Bus = pldBus },
                new Seat { SeatNumber = "A6", Bus = pldBus },
                new Seat { SeatNumber = "A1", Bus = pzBus },
                new Seat { SeatNumber = "A2", Bus = pzBus },
                new Seat { SeatNumber = "A3", Bus = pzBus },
                new Seat { SeatNumber = "A4", Bus = pzBus },
                new Seat { SeatNumber = "A5", Bus = pzBus },
                new Seat { SeatNumber = "A6", Bus = pzBus }
            };

            foreach (var seat in seats)
            {
                context.Seats.Add(seat);
            }

            context.SaveChanges();

            var lines = new Line[]
            {
                new Line { LineName = "Sofia - Varna" },
                new Line { LineName = "Sofia - Plovdiv" },
                new Line { LineName = "Pazardjik - Plovdiv" },
            };

            foreach (var line in lines)
            {
                context.Lines.Add(line);
            }
            context.SaveChanges();

            var sfVnLine = context.Lines.First(l => l.LineName == "Sofia - Varna");
            var pzPldLine = context.Lines.First(l => l.LineName == "Pazardjik - Plovdiv");
            var sfPldLine = context.Lines.First(l => l.LineName == "Sofia - Plovdiv");

            var stops = new Stop[]
            {
                new Stop { StopName = "Sofia" },
                new Stop { StopName = "Pazardjik" },
                new Stop { StopName = "Varna" },
                new Stop { StopName = "Plovdiv" }
            };

            foreach (var stop in stops)
            {
                context.Stops.Add(stop);
            }
            context.SaveChanges();

            var schedules = new Schedule[]
            {
                new Schedule
                {
                    LineId = sfVnLine.Id,
                    DepartureTime = new TimeSpan(7, 30, 0),
                    Bus = sfBus
                },
                new Schedule
                {
                    LineId = sfVnLine.Id,
                    DepartureTime = new TimeSpan(13, 30, 0),
                    Bus = sfBus
                },
                new Schedule
                {
                    LineId = pzPldLine.Id,
                    DepartureTime = new TimeSpan(19, 30, 0),
                    Bus = pzBus
                },
                new Schedule
                {
                    LineId = pzPldLine.Id,
                    DepartureTime = new TimeSpan(8, 30, 0),
                    Bus = pzBus
                },
                new Schedule
                {
                    LineId = pzPldLine.Id,
                    DepartureTime = new TimeSpan(20, 30, 0),
                    Bus = pzBus
                },
                new Schedule
                {
                    LineId = sfPldLine.Id,
                    DepartureTime = new TimeSpan(8, 30, 0),
                    Bus = pldBus
                },
                new Schedule
                {
                    LineId = sfPldLine.Id,
                    DepartureTime = new TimeSpan(20, 45, 0),
                    Bus = pldBus
                }
            };

            foreach (var schedule in schedules)
            {
                context.Schedules.Add(schedule);
            }
            context.SaveChanges();

            var cityStations = new CityStation[]
            {
                new CityStation { StationName = "Sofia-Central"},
                new CityStation { StationName = "Sofia-North"},
                new CityStation { StationName = "Sofia-South"},
                new CityStation { StationName = "Pazardjik-Center"},
                new CityStation { StationName = "Plovdiv-Central"},
                new CityStation { StationName = "Plovdiv-South"},
                new CityStation { StationName = "Varna-Central"},
                new CityStation { StationName = "Varna-North"},
                new CityStation { StationName = "Varna-East"}
            };

            foreach (var cityStation in cityStations)
            {
                context.CityStations.Add(cityStation);
            }
            context.SaveChanges();

            var lineStops = new LineStopStation[]
            {
                new LineStopStation {
                    Line = sfVnLine,
                    LineId = sfVnLine.Id,
                    Stop = context.Stops.First(s => s.StopName == "Sofia"),
                    StopId = context.Stops.First(s => s.StopName == "Sofia").Id,
                    CityStation = context.CityStations.First(s => s.StationName == "Sofia-Central"),
                    CityStationId = context.CityStations.First(s => s.StationName == "Sofia-Central").Id,
                    Order = 0,
                    PriceToGetThere = 0M,
                    IsLastStop = false,
                    TimeToReachFromTheFirstLineStop = new TimeSpan(0, 0, 0)
                },
                new LineStopStation {
                    Line = sfVnLine,
                    LineId = sfVnLine.Id,
                    Stop = context.Stops.First(s => s.StopName == "Pazardjik"),
                    StopId = context.Stops.First(s => s.StopName == "Pazardjik").Id,
                    CityStation = context.CityStations.First(s => s.StationName == "Pazardjik-Center"),
                    CityStationId = context.CityStations.First(s => s.StationName == "Pazardjik-Center").Id,
                    Order = 1,
                    PriceToGetThere = 3.20M,
                    IsLastStop = false,
                    TimeToReachFromTheFirstLineStop = new TimeSpan(1, 42, 0)
                },
                new LineStopStation {
                    Line = sfVnLine,
                    LineId = sfVnLine.Id,
                    Stop = context.Stops.First(s => s.StopName == "Plovdiv"),
                    StopId = context.Stops.First(s => s.StopName == "Plovdiv").Id,
                    CityStation = context.CityStations.First(s => s.StationName == "Plovdiv-Central"),
                    CityStationId = context.CityStations.First(s => s.StationName == "Plovdiv-Central").Id,
                    Order = 2,
                    PriceToGetThere = 2.8M,
                    IsLastStop = false,
                    TimeToReachFromTheFirstLineStop = new TimeSpan(2, 26, 0)
                },
                new LineStopStation {
                    Line = sfVnLine,
                    LineId = sfVnLine.Id,
                    Stop = context.Stops.First(s => s.StopName == "Varna"),
                    StopId = context.Stops.First(s => s.StopName == "Varna").Id,
                    CityStation = context.CityStations.First(s => s.StationName == "Varna-Central"),
                    CityStationId = context.CityStations.First(s => s.StationName == "Varna-Central").Id,
                    Order = 3,
                    PriceToGetThere = 10M,
                    IsLastStop = true,
                    TimeToReachFromTheFirstLineStop = new TimeSpan(6, 45, 0)
                },
                new LineStopStation {
                    Line = pzPldLine,
                    LineId = pzPldLine.Id,
                    Stop = context.Stops.First(s => s.StopName == "Pazardjik"),
                    StopId = context.Stops.First(s => s.StopName == "Pazardjik").Id,
                    CityStation = context.CityStations.First(s => s.StationName == "Pazardjik-Center"),
                    CityStationId = context.CityStations.First(s => s.StationName == "Pazardjik-Center").Id,
                    Order = 0,
                    PriceToGetThere = 0M,
                    IsLastStop = false,
                    TimeToReachFromTheFirstLineStop = new TimeSpan(0, 0, 0)
                },
                new LineStopStation {
                    Line = pzPldLine,
                    LineId = pzPldLine.Id,
                    Stop = context.Stops.First(s => s.StopName == "Plovdiv"),
                    StopId = context.Stops.First(s => s.StopName == "Plovdiv").Id,
                    CityStation = context.CityStations.First(s => s.StationName == "Plovdiv-South"),
                    CityStationId = context.CityStations.First(s => s.StationName == "Plovdiv-South").Id,
                    Order = 1,
                    PriceToGetThere = 1.10M,
                    IsLastStop = true,
                    TimeToReachFromTheFirstLineStop = new TimeSpan(0, 30, 0)
                },

                new LineStopStation {
                    Line = sfPldLine,
                    LineId = sfPldLine.Id,
                    Stop = context.Stops.First(s => s.StopName == "Sofia"),
                    StopId = context.Stops.First(s => s.StopName == "Sofia").Id,
                    CityStation = context.CityStations.First(s => s.StationName == "Sofia-North"),
                    CityStationId = context.CityStations.First(s => s.StationName == "Sofia-North").Id,
                    Order = 0,
                    PriceToGetThere = 0M,
                    IsLastStop = false,
                    TimeToReachFromTheFirstLineStop = new TimeSpan(0, 0, 0)
                },
                new LineStopStation {
                    Line = sfPldLine,
                    LineId = sfPldLine.Id,
                    Stop = context.Stops.First(s => s.StopName == "Pazardjik"),
                    StopId = context.Stops.First(s => s.StopName == "Pazardjik").Id,
                    CityStation = context.CityStations.First(s => s.StationName == "Pazardjik-Center"),
                    CityStationId = context.CityStations.First(s => s.StationName == "Pazardjik-Center").Id,
                    Order = 1,
                    PriceToGetThere = 2.15M,
                    IsLastStop = false,
                    TimeToReachFromTheFirstLineStop = new TimeSpan(1, 20, 0)
                },
                new LineStopStation {
                    Line = sfPldLine,
                    LineId = sfPldLine.Id,
                    Stop = context.Stops.First(s => s.StopName == "Plovdiv"),
                    StopId = context.Stops.First(s => s.StopName == "Plovdiv").Id,
                    CityStation = context.CityStations.First(s => s.StationName == "Plovdiv-South"),
                    CityStationId = context.CityStations.First(s => s.StationName == "Plovdiv-South").Id,
                    Order = 2,
                    PriceToGetThere = 0.8M,
                    IsLastStop = true,
                    TimeToReachFromTheFirstLineStop = new TimeSpan(2, 0, 0)
                }
            };

            foreach (var lineStop in lineStops)
            {
                context.LineStopStations.Add(lineStop);
            }
            context.SaveChanges();

            var user = new User
            {
                Email = "example-email@example.com",
                Password = "1",
                Phone = "555-55-5555"
            };
            context.Users.Add(user);
            context.SaveChanges();
        }
    }
}

namespace PandaTour.Data
{
    using PandaTour.Models;
    using Microsoft.EntityFrameworkCore;

    public class PandaTourContext : DbContext
    {
        public PandaTourContext(DbContextOptions<PandaTourContext> options) : base(options)
        {
            
        }

        public DbSet<Bus> Buses { get; set; }
        public DbSet<Line> Lines { get; set; }
        public DbSet<LineStopStation> LineStopStations { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Stop> Stops { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<CityStation> CityStations { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<TicketStop> TicketStops { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bus>().ToTable("Bus");
            modelBuilder.Entity<Line>().ToTable("Line");
            modelBuilder.Entity<LineStopStation>().ToTable("LineStopStation");
            modelBuilder.Entity<Seat>().ToTable("Seat");
            modelBuilder.Entity<Stop>().ToTable("Stop");
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Ticket>().ToTable("Ticket");
            modelBuilder.Entity<CityStation>().ToTable("CityStation");
            modelBuilder.Entity<Schedule>().ToTable("Schedule");
            modelBuilder.Entity<TicketStop>().ToTable("TicketStop");
            modelBuilder.Entity<Comment>().ToTable("Comment");

            modelBuilder.Entity<TicketStop>()
                .HasOne(t => t.Ticket)
                .WithMany(ts => ts.TicketStops)
                .OnDelete(DeleteBehavior.Restrict);

            // This could be a problem if we have situation where somene reserves a seat to an end
            // point of LineStopStationId and another start from the same point. Then the constrain will
            // be violated but it would be a valid resevation. Possible solution would be not to save the 
            modelBuilder.Entity<TicketStop>()
                .HasIndex(ts => new { ts.ScheduleId, ts.LineStopStationId, ts.ForDate, ts.SeatId })
                .IsUnique();

            modelBuilder.Entity<TicketStop>()
                .HasOne(s => s.Schedule)
                .WithMany(ts => ts.TicketStops)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TicketStop>()
                .HasOne(s => s.Seat)
                .WithMany(ts => ts.TicketStops)
                .OnDelete(DeleteBehavior.Restrict);

            // Guarantees that the user can't write more than one comment for trip
            // However we need to handle in code the case when the trip hasn't happened yet.
            modelBuilder.Entity<Comment>()
                .HasIndex(c => new { c.TicketId, c.UserId })
                .IsUnique(true);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Ticket>()
                .HasIndex(t => t.ForDate)
                .IsUnique(false)
                .ForSqlServerIsClustered(false);

            modelBuilder.Entity<LineStopStation>()
                .HasIndex(ls => ls.Order)
                .IsUnique(false)
                .ForSqlServerIsClustered(false);
        }
    }
}

using Air.Domain.Fares.Models;
using Microsoft.EntityFrameworkCore;

namespace Air.Domain.Fares.DataLayer.Services
{
    public class DbContextService : DbContext
    {
        public DbContextService() { }

        public DbContextService(DbContextOptions<DbContextService> options) : base(options)
        {
        }

        public DbSet<FlightFare> FlightFares { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<FlightFare>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Airline).IsRequired();
                entity.Property(e => e.Currency).IsRequired();
                entity.Property(e => e.Origin).IsRequired();
                entity.Property(e => e.Destination).IsRequired();
                entity.Property(e => e.Amount).IsRequired();
                entity.Property(e => e.PublishedFare).IsRequired();
                entity.Property(e => e.FlightNumber).IsRequired();
                entity.Property(e => e.CustomAirlineID).IsRequired();
                entity.Property(e => e.Departure).IsRequired();
                entity.Property(e => e.DepartureUtc).IsRequired();
                entity.Property(e => e.Arrival).IsRequired();
                entity.Property(e => e.ArrivalUtc).IsRequired();
                entity.Property(e => e.CollectedUtc).IsRequired();
                entity.Property(e => e.FaresLeft).IsRequired(false);
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (Environment.GetEnvironmentVariable("USE_INMEMORY_DB")?.ToUpper() == "TRUE")
            {
                optionsBuilder.UseInMemoryDatabase("TenStarDb");
                return;
            }

            if (!optionsBuilder.IsConfigured)
            {
                var sqlitePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Air", "air.db");
            }
        }
    }
}

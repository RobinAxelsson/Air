using Microsoft.EntityFrameworkCore;

namespace Air.Domain;

public class AirDbContext : DbContext
{
    public AirDbContext()
    {
    }

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public AirDbContext(DbContextOptions<AirDbContext> options) : base(options)
    {
    }

    public DbSet<FlightFare> FlightFares { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<FlightFare>(entity =>
        {
            entity.HasKey(e => e.Id);
            //entity.Property(e => e.Airline).IsRequired()
            entity.Property(e => e.Currency).IsRequired();
            entity.Property(e => e.Origin).IsRequired();
            entity.Property(e => e.Destination).IsRequired();
            entity.Property(e => e.Fare).IsRequired();
            //entity.Property(e => e.PublishedFare).IsRequired();
            entity.Property(e => e.FlightNumber).IsRequired();
            //entity.Property(e => e.CustomAirlineID).IsRequired();
            entity.Property(e => e.Departure).IsRequired();
            //entity.Property(e => e.DepartureUtc).IsRequired();
            entity.Property(e => e.Arrival).IsRequired();
            entity.Property(e => e.AirCreated).IsRequired();
            //entity.Property(e => e.ArrivalUtc).IsRequired();
            //entity.Property(e => e.CollectedUtc).IsRequired();
            //entity.Property(e => e.FaresLeft).IsRequired(false);
        });
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost;Database=Air.Db;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True;");
        //optionsBuilder.UseInMemoryDatabase("AirFaresDb");
    }
}

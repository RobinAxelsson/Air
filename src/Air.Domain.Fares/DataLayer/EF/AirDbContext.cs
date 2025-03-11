using Microsoft.EntityFrameworkCore;

namespace Air.Domain;

internal class AirDbContext : DbContext
{
    private readonly string? _dbConnectionString;

    [Obsolete("Only needed for entity framework", true)]
    public AirDbContext(){}
    public AirDbContext(string dbConnectionString)
    {
        _dbConnectionString = dbConnectionString;
    }

    [Obsolete("Only needed for entity framework", true)]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal AirDbContext(DbContextOptions<AirDbContext> options) : base(options)
    {
    }

    public DbSet<AirFlight> AirFlights { get; set; }
    public DbSet<AirFare> AirFares { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AirFlight>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Airline)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.FlightNumber)
                .IsRequired()
                .HasMaxLength(10);

            entity.Property(e => e.Origin)
                .IsRequired()
                .HasMaxLength(5);

            entity.Property(e => e.Destination)
                .IsRequired()
                .HasMaxLength(5);

            entity.Property(e => e.DepartureUtc)
                .IsRequired();

            entity.Property(e => e.ArrivalUtc)
                .IsRequired();

            entity.Property(e => e.CreatedUtc)
                .HasDefaultValueSql("GETUTCDATE()");

            entity.HasMany(e => e.Fares)
                .WithOne()
                .HasForeignKey("AirFlightId")
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<AirFare>(entity =>
        {
            entity.HasKey(e => e.FlightFareId);

            entity.Property(e => e.Fare)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            entity.Property(e => e.Source)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.CreatedUtc)
                .HasDefaultValueSql("GETUTCDATE()");

            entity.Property(e => e.LastObservedUtc)
                .HasDefaultValueSql("GETUTCDATE()");
        });

        // Optional: Configuring indexes
        modelBuilder.Entity<AirFlight>()
            .HasIndex(e => new { e.FlightNumber, e.DepartureUtc })
            .IsUnique();

        modelBuilder.Entity<AirFare>()
            .HasIndex(e => e.Source);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //We allow this to be used with nullable string for the dotnet ef migrations commmand
        optionsBuilder.UseSqlServer(_dbConnectionString);
    }
}

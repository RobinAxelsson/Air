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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AirFlight>(entity =>
        {
            entity.ToTable("FlightFares");

            // EF creates a shadow property "Id" and this to sepparate domain layer AirId from EF Id
            entity.Property<int>("Id")
                  .ValueGeneratedOnAdd();

            entity.HasKey("Id");

            //entity.HasIndex(e => e.AirId).IsUnique();
            entity.Property(e => e.Airline).IsRequired();
            //entity.Property(e => e.Currency).IsRequired();
            entity.Property(e => e.Origin).IsRequired();
            entity.Property(e => e.Destination).IsRequired();
            //entity.Property(e => e.Fare).IsRequired();
            entity.Property(e => e.FlightNumber).IsRequired();
            entity.Property(e => e.DepartureUtc).IsRequired();
            entity.Property(e => e.ArrivalUtc).IsRequired();
        });
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //We allow this to be used with nullable string for the dotnet ef migrations commmand
        optionsBuilder.UseSqlServer(_dbConnectionString);
    }
}

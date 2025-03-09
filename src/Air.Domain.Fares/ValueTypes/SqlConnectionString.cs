namespace Air.Domain;

internal sealed record SqlConnectionString
{
    public required string Host { get; init; }
    public required int Port { get; init; }
    public required string Database { get; init; }
    public required string UserId { get; init; }
    public required string Password { get; init; }
    public bool TrustServerCertificate { get; init; }
    public string ToConnectionStringWithoutDatabase() =>
       $"Server={Host},{Port};User Id={UserId};Password={Password};TrustServerCertificate={TrustServerCertificate};";
    public override string ToString() =>
        $"Server={Host},{Port};Database={Database};User Id={UserId};Password={Password};TrustServerCertificate={TrustServerCertificate};";
}

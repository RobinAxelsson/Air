#!/usr/bin/env pwsh

#validate args
if($args.Length -eq 0 -or $args.Length -gt 1 -or ($args -notcontains "update" -and $args -notcontains "drop")){
    Write-Host "Usage: ef-testdb <update|drop>"
    exit 1
}

$connectionString = "Server=localhost;Database=Air.TestDb;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True;"

if($args.Contains("update")){
    dotnet-ef database update --startup-project $env:_EF_PROJ_PATH_ --connection $connectionString
    exit 0
}

if($args.Contains("drop")){
    sqlcmd -S localhost -U sa -P "YourStrong@Passw0rd" -d master -Q "
    ALTER DATABASE [Air.TestDb] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE [Air.TestDb];
    "
    Write-Host "Database Air.TestDb dropped successfully."
    exit 0
}

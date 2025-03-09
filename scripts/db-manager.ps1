#!/usr/bin/env pwsh
$ErrorActionPreference = 'Stop'

function Show-Usage {
    Write-Host "Usage: db-manager <update|drop|list-migrations|view-fares> [--use-test|--use-prod]"
    exit 1
}

if ($args.Length -lt 2 -or $args.Length -gt 2) {
    Show-Usage
}

$action = $args[0]
$envOption = $args[1]

if (($action -ne "update" -and $action -ne "drop" -and $action -ne "view-fares" -and $action -ne "view-tables"  -and $action -ne "list-migrations") -or
    ($envOption -ne "--use-test" -and $envOption -ne "--use-prod")) {
    Show-Usage
}

$db = if ($envOption -eq "--use-test") { "Air.TestDb" } else { "Air.ProdDb" }

$connectionString = "Server=localhost;Database=$db;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True;"

if ($action -eq "update") {
    $confirmation = Read-Host -Prompt "Are you sure you want to update the database '$db' with the latest migrations from the Entity Framework project? (y/n)"

    if ($confirmation -ne "y") {
        Write-Host "Update aborted."
        exit 0
    }

    dotnet ef database update --startup-project $env:_EF_PROJ_PATH_ --connection $connectionString
    exit 0
}

if($action -eq "list-migrations"){
    dotnet ef migrations list --project $env:_EF_PROJ_PATH_ --connection $connectionString
    exit 0
}

if ($action -eq "drop") {
    $confirmation = Read-Host -Prompt "Are you SURE you want to DROP the database '$db'? Type the database name to confirm"

    if ($confirmation -eq $db) {
        sqlcmd -S localhost -U sa -P "YourStrong@Passw0rd" -d master -Q "
        ALTER DATABASE [$db] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
        DROP DATABASE [$db];
        "
        Write-Host "Database '$db' dropped successfully."
    }
    else {
        Write-Host "Database drop aborted. Confirmation did not match."
        exit 1
    }
}

if ($action -eq "view-fares") {
    Write-Host "Viewing flight fares from database '$db'..."

    $query = "SELECT * FROM dbo.FlightFares;"

    $conn = New-Object System.Data.SqlClient.SqlConnection($connectionString)

    try {
        $conn.Open()
        $cmd = $conn.CreateCommand()
        $cmd.CommandText = $query
        $adapter = New-Object System.Data.SqlClient.SqlDataAdapter $cmd
        $dataset = New-Object System.Data.DataSet
        $adapter.Fill($dataset)

        $dataset.Tables[0] | Format-Table -AutoSize
    }
    catch {
        Write-Host "Error: $_" -ForegroundColor Red
    }
    finally {
        $conn.Close()
    }
}

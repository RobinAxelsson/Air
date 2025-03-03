#!/usr/bin/env pwsh

if ($args.length -eq 0) {
    Write-Host "Usage: ef-migrate <migration-name>"
    Write-Host "Listing current migrations..."
    dotnet-ef migrations list --project $env:_EF_PROJ_PATH_
    exit 1
}

$migrationsDir = Join-Path $env:_EF_PROJ_PATH_ DataLayer EF Migrations

if (-not (Test-Path $migrationsDir)) {
    Write-Host "migrationsfolder not found in path: $migrationsDir"
    exit 1
}

$confirmation = Read-Host "Are you sure you want to add a new migration named '$($args[0])'? (y/n)"
if ($confirmation -ne "y") {
    Write-Host "Aborted."
    exit 1
}

dotnet-ef migrations add --startup-project $env:_EF_PROJ_PATH_ $args[0] --output-dir $migrationsDir

# To undo this action, use 'ef migrations remove'

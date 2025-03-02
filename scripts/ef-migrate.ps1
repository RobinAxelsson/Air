#!/usr/bin/env pwsh

if ($args.length -eq 0) {
    Write-Host "Usage: ef-migrate <migration-name>"
    Write-Host "Listing current migrations..."
    dotnet-ef migrations list --project $env:_EF_PROJ_PATH_
    exit 1
}

dotnet-ef migrations add --startup-project $env:_EF_PROJ_PATH_ $args[0]

# To undo this action, use 'ef migrations remove'

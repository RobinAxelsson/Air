#!/usr/bin/env pwsh
$ErrorActionPreference = 'Stop'

function Show-Usage {
    Write-Host "Usage: ef-migration <create|remove|list>"
    Write-Host "Input arguments will be prompted, only provide action."
    exit 1
}

function NormalizePath {
    [CmdletBinding()]
    param (
        [Parameter(Mandatory = $true)]
        [string] $path
    )

    $separator = [IO.Path]::DirectorySeparatorChar
    $invalidSeparator = [IO.Path]::AltDirectorySeparatorChar
    $path = $path -replace $invalidSeparator, $separator

    return $path
}


$action = $args[0]
if($args.Length -ne 1){
    Show-Usage
}

if ($action -ne "create" -and $action -ne "remove" -and $action -ne "list") {
    Show-Usage
    exit 1
}

if ($action -eq "list") {
    dotnet ef migrations list --project $env:_EF_PROJ_PATH_

    exit 0
}

if($action -eq "remove"){
   $name = Read-Host "Name your migration - use Pascal case"
   $confirmation = Read-Host "Remove migration: '$name'? (y/n)"
    if ($confirmation -ne "y") {
         Write-Host "Aborted."
         exit 0
    }
    dotnet ef migrations remove --project $env:_EF_PROJ_PATH_ --name $name
}

if($action -eq "create"){
    $migrationsDir = NormalizePath "$env:_EF_PROJ_PATH_/DataLayer/EF/Migrations"

    if (-not (Test-Path $migrationsDir)) {
        Write-Host "Migrationsfolder not found in path: $migrationsDir"
        Write-Host "Creating migrations folder..."
        New-Item -Path $migrationsDir -ItemType Directory | Out-Null
    }

    $name = Read-Host "Name your migration - use Pascal case"
    $confirmation = Read-Host "Create migration named: '$name'? (y/n)"
    if ($confirmation -ne "y") {
        Write-Host "Aborted."
        exit 0
    }

    dotnet-ef migrations add --startup-project $env:_EF_PROJ_PATH_ $name --output-dir $migrationsDir
    if($LASTEXITCODE -ne 0){
        Write-Host "Migration creation failed. Exiting..."
        exit 1
    }

    Write-Host "Migration '$name' created successfully. Remove with 'db-manager remove-migration --use-test"
    exit 0
}

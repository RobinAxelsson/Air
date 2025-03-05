#!/usr/bin/env pwsh

param(
    [required]
    [string]$action)

$ErrorActionPreference = 'Stop'

function Main(){
    if(-not $action){
        throw "Usage: docker-db <action> [--update-db]"
    }

    $dockerCompose = Join-Path $PSScriptRoot subs docker-compose.db.yml

    if (-not (Test-Path $dockerCompose)) {
        throw "script not found in path: $migrationsDir"
    }

    docker-compose -f $dockerCompose up -d

    if($args -contains "--update-db"){
        Write-Host "$ Updating database..."
        WaitForTcp 1433
        Start-Sleep 2
        dotnet-ef database update --startup-project $env:_EF_PROJ_PATH_
    }
}

function WaitForTcp($port){
    for ($i = 0; $i -lt 3; $i++) {
        if($result) {
            break;
        }
    }
}

Main

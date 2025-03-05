#!/usr/bin/env pwsh
$ErrorActionPreference = 'Stop'

$airWatchTool = "AirWatchTool"

Write-Host "Checking if $airWatchTool is installed"
$toolList = dotnet tool list --global | Out-String

if ($toolList -match $airWatchTool) {
    Write-Output "Uninstalling the $airWatchTool"
    dotnet tool uninstall --global $airWatchTool
}

$outDir = Join-Path $PSScriptRoot "out"

dotnet pack -c Release -o $outDir

if($LASTEXITCODE -ne 0){
    throw "Could not pack the project"
}

dotnet tool install --global --add-source $outDir AirwatchTool

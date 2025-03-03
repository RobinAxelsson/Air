#!/usr/bin/env pwsh

$scriptsPath = Join-Path $PSScriptRoot scripts
$endToEndIntegrationTests = Join-Path $PSScriptRoot test EndToEndIntegrationTests
$airDomainFaresPath = Join-Path $PSScriptRoot src Air.Domain.Fares
$airInterfaceCliPath = Join-Path $PSScriptRoot src Air.Interface.CLI

EnsurePathExists $airDomainFaresPath
EnsurePathExists $airInterfaceCliPath
EnsurePathExists $scriptsPath
EnsurePathExists $endToEndIntegrationTests

# add script paths to run dev scripts
AddDirToPath $scriptsPath
AddDirToPath $endToEndIntegrationTests

# add environment variables (some scripts depend on these)
$env:_REPO_NAME_ = "Air"
$env:_REPO_ROOT_ = $PSScriptRoot
$env:_EF_PROJ_PATH_ = $airDomainFaresPath
$env:_CLI_PROJ_PATH_ = $airInterfaceCliPath

function AddDirToPath($dir){
    if (-not ($env:Path -split [System.IO.Path]::PathSeparator -contains $dir)) {
        $env:Path += [System.IO.Path]::PathSeparator + $dir
    }
}

function EnsurePathExists($dir){
    if (-not (Test-Path $dir)) {
        throw "Path does not exist: $dir"
    }
}

#!/usr/bin/env pwsh

function AddDirToPath($dir){
    $fullDir = Join-Path $PSScriptRoot $dir

    if (-not ($env:Path -split $pathSep -contains $fullDir)) {
        $env:Path += [System.IO.Path]::PathSeparator + $fullDir
    }
}

AddDirToPath "scripts"
AddDirToPath "test\EndToEndIntegrationTests"

$env:_REPO_NAME_ = "Air"

$env:_EF_PROJ_PATH_ = Join-Path $PSScriptRoot "src" "Air.Domain.Fares"
$env:_CLI_PROJ_PATH_ = Join-Path $PSScriptRoot "src" "Air.Interface.CLI"

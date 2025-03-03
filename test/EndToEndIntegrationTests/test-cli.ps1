#!/usr/bin/env pwsh

$out_dir = Join-Path $PSScriptRoot "out" "cli"

Remove-Item -Recurse -Force $out_dir

if (-not (Test-Path $out_dir)) {
    New-Item -ItemType Directory -Path $out_dir | Out-Null
}

dotnet build ${env:_CLI_PROJ_PATH_} -c Release -o $out_dir

$exe = Join-Path $out_dir "Air.Interface.CLI.exe"

if($args.Length -eq 1){
    $coveragePath = $args[0]
    dotnet-coverage collect -f cobertura -o $coveragePath -- $exe
}

sql-select-fares.ps1

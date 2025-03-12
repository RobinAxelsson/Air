#!/usr/bin/env pwsh
$ErrorActionPreference = 'Stop'
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

# Generates a test coverage report for both dotnet and pwsh tests and creates a merged html report in $report_dir
# Since we dont want to see this as often as normal test run we dont need a watch script for this
$report_dir = NormalizePath "$env:_REPO_ROOT_/report/coverage"

$coverageDotnetTestsPath = NormalizePath "$env:_REPO_ROOT_/temp/test-coverage/dotnet.cobertura.xml"
$coveragePwshTestsPath   = NormalizePath "$env:_REPO_ROOT_/temp/test-coverage/pwsh.cobertura.xml"

dotnet-coverage collect -f cobertura -o $coverageDotnetTestsPath -- dotnet test
test-cli.ps1 $coveragePwshTestsPath

if(-not (Test-Path $coverageDotnetTestsPath)){
    throw "$coverageDotnetTestsPath not found"
}

if(-not (Test-Path $coveragePwshTestsPath)){
    throw "$coveragePwshTestsPath not found"
}

dotnet-coverage merge $coverageDotnetTestsPath $coveragePwshTestsPath -o $tempMerged -f cobertura

if (Test-Path $report_dir) {
    Remove-Item $(NormalizePath "$report_dir/*") -Recurse -Force -Exclude "index.html"
}

Write-Host "Generating report..."
reportgenerator -reports:"$tempMerged" -targetdir:"$report_dir"

#!/usr/bin/env pwsh

# create a varible for a filename where the datetime is the name

if($args.Length -eq 0 -or $args.Length -gt 1 -or ($args -notcontains "serve" -and $args -notcontains "collect")){
    Write-Host "Usage: test-coverage <collect|serve>"
    exit 1
}

$report_dir = Join-Path $env:_REPO_ROOT_ "coverage-report"

if ($args[0] -eq "collect") {
    $coveragePath = Join-Path $env:_REPO_ROOT_ test "TestCoverage" "$(Get-Date -f yyyy-MM-dd_HHmm).cobertura.xml"
    $tempXunit = Join-Path $env:_REPO_ROOT_ temp "TestCoverage" "xunit.cobertura.xml"
    $tempCli = Join-Path $env:_REPO_ROOT_ temp "TestCoverage" "cli.cobertura.xml"

    dotnet-coverage collect -f cobertura -o $tempXunit -- dotnet test
    test-cli.ps1 $tempCli

    if(-not (Test-Path $tempXunit)){
        Write-Host $tempXunit "not found"
    }

    if(-not (Test-Path $tempCli)){
        Write-Host $tempCli "not found"
    }

    if(-not (Test-Path $tempXunit) -or -not (Test-Path $tempCli)){
        exit 1
    }

    dotnet-coverage merge $tempXunit $tempCli -o $coveragePath -f cobertura

    if (Test-Path $report_dir) {
        Remove-Item $report_dir -Recurse -Force
    }

    Write-Host "Generating report..."
    reportgenerator -reports:"$coveragePath" -targetdir:coverage-report
    exit(0)
}

if ($args[0] -eq "serve") {
    Write-Host "Starting server on localhost:8833..."
    python -m http.server 8833 --directory $report_dir
    exit(0)
}

Write-Host "Not implemented exception"
exit 1

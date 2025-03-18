#!/usr/bin/env pwsh
# dependencies
# dotnet add package JunitXml.TestLogger
# pip install junit2html
# WIP

$testResultsDir = Join-Path $env:_REPO_ROOT_ temp "test-results"

if (Test-Path $testResultsDir) {
    Remove-Item $(Join-Path $testResultsDir "*.xml") -Recurse -Force
}

$dotnetTestPath = Join-Path $testResultsDir "dotnet-test.xml"
dotnet test --logger:"junit;LogFilePath=$($dotnetTestPath)"
run-pester.ps1 --output-junit

$reportDir = Join-Path $env:_REPO_ROOT_ report testing
if(-not $(Test-Path $reportDir)){
    New-Item -ItemType Directory -Path $reportDir
}

$xmlFiles = Get-ChildItem -Path $testResultsDir -Filter "*.xml"

if($xmlFiles.Count -eq 0){
    Write-Host "⚠️ No xml files exist."
    exit 1
}

[xml]$mergedXml = Get-Content $xmlFiles[0].FullName

# to be able to run different cross process tests, we need to merge all the xml files
# merge all the xml files if there are more than one
if ($xmlFiles.Count -gt 1) {
    foreach ($file in $xmlFiles[1..$xmlFiles.Count]) {
        [xml]$currentXml = Get-Content $file.FullName
        $suites = $currentXml.testsuites.ChildNodes
        foreach ($suite in $suites) {
            $importedNode = $mergedXml.ImportNode($suite, $true)
            $mergedXml.testsuites.AppendChild($importedNode) | Out-Null
        }
    }
}

$mergedXmlPath = Join-Path $testResultsDir "$(Get-Date -f yyyy-MM-dd_HHmm).merged.tests.xml"
$mergedXml.Save($mergedXmlPath)
Write-Host "✅ Merged JUnit XML saved as $mergedXmlPath"

$reportOutputPath = Join-Path $reportDir "index.html"
junit2html $mergedXmlPath $reportOutputPath

Write-Host "✅ HTML report generated: $outputHtmlPath"

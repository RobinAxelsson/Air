#!/usr/bin/env pwsh

$db = Join-Path $PSScriptRoot docker-compose.database.yml
docker-compose -f $db down

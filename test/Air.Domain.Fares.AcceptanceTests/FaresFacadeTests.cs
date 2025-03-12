// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.Json;
using Xunit.Abstractions;

namespace Air.Domain.Fares.Tests.AcceptanceTests
{
    public class FaresFacadeTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public FaresFacadeTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Trait("", "AcceptanceTest")]
        [Fact]
        public async Task SyncFares_WhenCalled_xFaresUpdatedInDb()
        {
            /*
             * Give me new Fares for the configured surf routes and output it to a file
             *
             * Steps:
             * User of system calls SyncFares
             * Create ryan air request logic
             * SyncFares calls Ryanair API
             * Creates Valid FlightFares Models
             * DataLayer persists to the database
             *
             * Dev input:
             * airport codes
             * Date(s)
             * urls
             *
             * External ~input:
             * Ryanair API response
             *
             * User input:
             * No user input
             *
             * Persistance:
             * No permanent persistance - json file is stored in temp
             *
             * Dependencies:
             * OS File System
             * Ryanair API availability, urls, request limits
             *
             * Output Happy path:
             * 1. All expected fares gets written to an output file
             * 2. All requests work and found fares gets written to an output file
             *
             * Error path:
             * - Ryanair http responses else then 200
             * - File system errors, could not write, access denied, os errors unix, windows
             * - Invalid dev input: airport codes invalid type, date strings invalid type
             *
             *
            */

            var faresFacade = new FaresFacade();

            var syncFlightFaresResult = await faresFacade.SyncFlightFares(new FlightSpecDto() {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(7)),
                Origin = AirportCode.GOT,
                Destination = AirportCode.STN,
            });

            Assert.True(syncFlightFaresResult.FlightsCreated > 0 || syncFlightFaresResult.FlightsUpdated > 0);
            var faresJson = JsonSerializer.Serialize(syncFlightFaresResult, new JsonSerializerOptions { WriteIndented = true });
            _testOutputHelper.WriteLine(faresJson);
        }
    }
}

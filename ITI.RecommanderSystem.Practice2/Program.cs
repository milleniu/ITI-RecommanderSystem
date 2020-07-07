using System;
using System.Collections.Generic;
using System.Diagnostics;
using ITI.RecommanderSystem.CSV;
using ITI.RecommanderSystem.Practice2.Models;
using System.Linq;

namespace ITI.RecommanderSystem.Practice2
{
    internal static class Program
    {
        private static void Main( string[] args )
        {
            var dataFolder = args[ 0 ];

            var airportsByCode = GetAirports( dataFolder );
            var flights = GetFlights( dataFolder );

            var people = new[]
            {
                ( "Seymour", "BOS" ),
                ( "Franny", "DAL" ),
                ( "Zooey", "CAK" ),
                ( "Walt", "MIA" ),
                ( "Buddy", "ORD" ),
                ( "Les", "OMA" )
            };

            const string destination = "LGA";

            var groupTravel = new GroupTravel( people, airportsByCode, flights, destination );
            groupTravel.Compute();
        }

        private static IDictionary<string, Airport> GetAirports( string dataFolder )
        {
            var airportsCSVConfiguration = CSVLoader.InitializeConfiguration
            (
                "|",
                configuration => configuration.RegisterClassMap<Airport.AirportMap>()
            );
            var airports = CSVLoader.ReadCsv<Airport>
            (
                dataFolder,
                "airports.txt",
                airportsCSVConfiguration
            ).ToArray();

            var airportsByCode = airports.Select( a => new KeyValuePair<string, Airport>( a.Code, a ) );
            return new Dictionary<string, Airport>( airportsByCode );
        }

        private static IDictionary<FlightPath, IList<FlightSchedule>> GetFlights( string dataFolder )
        {
            var flights = new Dictionary<FlightPath, IList<FlightSchedule>>( new FlightPath.Comparer() );

            var scheduleCSVConfiguration = CSVLoader.InitializeConfiguration
            (
                ",",
                configuration => configuration.RegisterClassMap<Schedule.ScheduleMap>()
            );
            var schedules = CSVLoader.ReadCsv<Schedule>
            (
                dataFolder,
                "small-schedule.txt",
                scheduleCSVConfiguration
            ).ToArray();

            foreach( var schedule in schedules )
            {
                var flightPath = new FlightPath( schedule.Departure, schedule.Arrival );
                var flightSchedule = new FlightSchedule( schedule.DepartureTime, schedule.ArrivalTime, schedule.Price );

                if( flights.TryGetValue( flightPath, out var flightPathSchedules ) )
                    flightPathSchedules.Add( flightSchedule );
                else
                    flights[ flightPath ] = new List<FlightSchedule> { flightSchedule };
            }

            return flights;
        }
    }
}

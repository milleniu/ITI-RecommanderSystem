using System;
using System.Collections.Generic;
using System.Diagnostics;
using ITI.RecommanderSystem.CSV;
using System.Linq;
using ITI.RecommanderSystem.Practice2.DataModels;
using ITI.RecommanderSystem.Practice2.Model;

namespace ITI.RecommanderSystem.Practice2
{
    public static class Program
    {
        private static void Main( string[] args )
        {
            var dataFolder = args[ 0 ];

            var airportsByCode = Airport.GetAirports( dataFolder );
            var flights = Schedule.GetSchedules( dataFolder );

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
            var (result, cost, iterations) = SimulatedAnnealingGroupTravelComputer.Compute( groupTravel );
            
            Console.WriteLine($"{cost} (iterations: {iterations}");
            groupTravel.PrintSchedule(result);
        }
    }
}

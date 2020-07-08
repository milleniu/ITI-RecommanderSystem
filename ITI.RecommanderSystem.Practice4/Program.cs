using System;
using System.Diagnostics;
using ITI.RecommanderSystem.Core.DataStructure;
using ITI.RecommanderSystem.Practice2;
using ITI.RecommanderSystem.Practice2.DataModels;
using ITI.RecommanderSystem.Practice2.Model;
using ITI.RecommanderSystem.Practice3;

namespace ITI.RecommanderSystem.Practice4
{
    internal static class Program
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

            const int iterationsCount = 100;
            Console.WriteLine( $"Iterations count: {iterationsCount}{Environment.NewLine}" );

            var sw = new Stopwatch();

            {
                var bestSimulatedAnnealing = new WeightedResult<int[]>( Array.Empty<int>(), int.MaxValue );
                var totalIterations = 0;
                for( var i = 0; i < iterationsCount; ++i )
                {
                    sw.Start();
                    var (result, cost, iterations) = SimulatedAnnealingGroupTravelComputer.Compute( groupTravel );
                    sw.Stop();

                    totalIterations += iterations;
                    if( bestSimulatedAnnealing.Weight > cost )
                        bestSimulatedAnnealing = new WeightedResult<int[]>( result, cost );
                }

                Console.WriteLine( "##### Simulated Annealing #####" );
                Console.WriteLine
                (
                    $"Average Iterations: {totalIterations / iterationsCount:#} " +
                    $"Average Durations: {sw.ElapsedMilliseconds / (float) iterationsCount}ms"
                );
                Console.WriteLine( $"Best: {bestSimulatedAnnealing.Weight}" );
                groupTravel.PrintSchedule( bestSimulatedAnnealing.Result );
            }

            sw.Reset();
            Console.WriteLine();

            {
                var bestSimulatedAnnealing = new WeightedResult<int[]>( Array.Empty<int>(), int.MaxValue );
                for( var i = 0; i < iterationsCount; ++i )
                {
                    sw.Start();
                    var (result, cost) = GeneticGroupTravelComputer.Compute( groupTravel );
                    sw.Stop();

                    if( bestSimulatedAnnealing.Weight > cost )
                        bestSimulatedAnnealing = new WeightedResult<int[]>( result, cost );
                }

                Console.WriteLine( "##### Genetic Optimization #####" );
                Console.WriteLine( $"Average Durations: {sw.ElapsedMilliseconds / (float) iterationsCount}ms" );
                Console.WriteLine( $"Best: {bestSimulatedAnnealing.Weight}" );
                groupTravel.PrintSchedule( bestSimulatedAnnealing.Result );
            }
        }
    }
}

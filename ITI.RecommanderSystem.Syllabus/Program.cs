using ITI.RecommanderSystem.Core;
using ITI.RecommanderSystem.MarkovChains;
using QuickGraph;
using System;
using System.Collections.Generic;

namespace ITI.RecommanderSystem.Syllabus
{
    internal class Program
    {
        private static void Main( string[] args )
        {
            SimpleSalesman();
        }

        private static void UserBasedSimilarities()
        {
            var ratings = new[ , ]
            {
                { 4, 3, 0, 0, 5, 0 },
                { 5, 0, 4, 0, 4, 0 },
                { 4, 0, 5, 3, 4, 0 },
                { 0, 3, 0, 0, 0, 5 },
                { 0, 4, 0, 0, 0, 4 },
                { 0, 0, 2, 4, 0, 5 }
            };

            const int userId = 1;
            var usersSimilarities = ActorSimilarities.ComputeActorsSimilarities( ratings );
            var similarUsers = ActorSimilarities.GetSimilarActors( userId, usersSimilarities, 2 );
            var recommendations = ActorSimilarities.GetActorBasedRecommendations( userId, ratings, similarUsers, 3 );

            foreach( var (id, score) in recommendations )
                Console.WriteLine( $"{id}: {score}" );
        }

        private static void ComputePIWithMonteCarlo()
        {
            string s;
            int n;

            do
            {
                Console.WriteLine( "Enter n (n > 0): " );
                s = Console.ReadLine();
            } while( !int.TryParse( s, out n ) || n <= 0 );

            var random = new Random();

            var c = 0;

            for( var i = 0; i < n; ++i )
            {
                var x = random.NextDouble();
                var y = random.NextDouble();

                if( x * x + y * y <= 1 )
                {
                    c += 1;
                }
            }

            var piApproximation = 4D * c / n;
            Console.WriteLine( $"PI approximation: {piApproximation:0.######}" );
        }

        private static void SimpleSalesman()
        {
            var graph = new BidirectionalGraph<string, Edge<string>>( false, 5, 10 );

            var edge12 = new Edge<string>( "1", "2" );
            var edge13 = new Edge<string>( "1", "3" );
            var edge14 = new Edge<string>( "1", "4" );
            var edge15 = new Edge<string>( "1", "5" );
            var edge23 = new Edge<string>( "2", "3" );
            var edge24 = new Edge<string>( "2", "4" );
            var edge25 = new Edge<string>( "2", "5" );
            var edge34 = new Edge<string>( "3", "4" );
            var edge35 = new Edge<string>( "3", "5" );
            var edge45 = new Edge<string>( "4", "5" );

            graph.AddVerticesAndEdgeRange
            (
                new[]
                {
                    edge12,
                    edge13,
                    edge14,
                    edge15,
                    edge23,
                    edge24,
                    edge25,
                    edge34,
                    edge35,
                    edge45
                }
            );

            var costs = new Dictionary<Edge<string>, int>( 10 )
            {
                [ edge12 ] = 6,
                [ edge13 ] = 6,
                [ edge14 ] = 1,
                [ edge15 ] = 4,
                [ edge23 ] = 8,
                [ edge24 ] = 3,
                [ edge25 ] = 5,
                [ edge34 ] = 6,
                [ edge35 ] = 4,
                [ edge45 ] = 2
            };

            var result = TravelingSalesmanProblem.Resolve( graph, costs );

            Console.WriteLine( $"Path: {string.Join( "->", result )}" );
        }
    }
}

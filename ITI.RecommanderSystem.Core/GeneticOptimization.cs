using System;
using System.Collections.Generic;
using System.Linq;
using ITI.RecommanderSystem.Core.DataStructure;

namespace ITI.RecommanderSystem.Core
{
    public static class GeneticOptimization
    {
        public static int[] ResolveInDomain
        (
            (int, int)[] domain,
            Func<IReadOnlyList<int>, float> costFunction,
            int populationSize = 50,
            int step = 1,
            float mutationProbability = 0.2F,
            float bestToKeepPercentage = 0.2F,
            int iterations = 100
        )
        {
            var random = new Random();
            var resultAscending = new WeightedResult<int[]>.WeightedResultAscending();
            var resultDescending = new WeightedResult<int[]>.WeightedResultDescending();

            var size = domain.Length;
            var population = new List<int[]>();

            int[] RandomSolution()
            {
                var randomSolution = new int[ size ];
                for( var i = 0; i < size; i++ )
                {
                    var (min, max) = domain[ i ];
                    randomSolution[ i ] = random.Next( min, max + 1 );
                }

                return randomSolution;
            }

            for( var i = 0; i < populationSize; i++ )
                population.Add( RandomSolution() );

            var bestKeptCount = (int) ( populationSize * bestToKeepPercentage );
            var worstKeptCount = (int) ( bestKeptCount / 2F );
            var kept = bestKeptCount + worstKeptCount;

            int[] Crossing( int[] a, int[] b )
            {
                var crossed = new int[ size ];

                var crossingIdx = random.Next( 0, size + 1 );
                if( crossingIdx == 0 )
                {
                    Array.Copy( a, crossed, size );
                }
                else if( crossingIdx == size )
                {
                    Array.Copy( b, crossed, size );
                }
                else
                {
                    Array.Copy( a, crossed, crossingIdx );
                    Array.Copy( b, crossingIdx, crossed, crossingIdx, size - crossingIdx );
                }

                return crossed;
            }

            int[] Mutation( int[] a )
            {
                var mutated = (int[]) a.Clone();

                var mutationIdx = random.Next( 0, size );
                var direction = random.Next( -step, step + 1 );
                var (min, max) = domain[ mutationIdx ];

                mutated[ mutationIdx ] = Math.Max( Math.Min( max, mutated[ mutationIdx ] + direction ), min );

                return mutated;
            }

            for( var i = 0; i < iterations; i++ )
            {
                var bestKeeper = new BestKeeper<WeightedResult<int[]>>( bestKeptCount, resultDescending );
                var worseKeeper = new BestKeeper<WeightedResult<int[]>>( worstKeptCount, resultAscending );
                foreach( var candidate in population )
                {
                    var cost = costFunction( candidate );
                    var weightedCandidate = new WeightedResult<int[]>( candidate, cost );
                    bestKeeper.Add( weightedCandidate );
                    worseKeeper.Add( weightedCandidate );
                }

                population.Clear();
                population.AddRange( bestKeeper.Select( result => result.Result ) );
                population.AddRange( worseKeeper.Select( result => result.Result ) );

                for( var j = 0; j < worstKeptCount; ++j )
                    population.Add( RandomSolution() );

                while( population.Count < populationSize )
                {
                    if( random.NextDouble() < mutationProbability )
                    {
                        var mutationIdx = random.Next( 0, kept );
                        population.Add( Mutation( population[ mutationIdx ] ) );
                    }
                    else
                    {
                        var firstParentIndex = random.Next( 0, kept );
                        var secondParentIndex = random.Next( 0, kept );
                        population.Add( Crossing( population[ firstParentIndex ], population[ secondParentIndex ] ) );
                    }
                }
            }

            var candidatesCost = population
                      .Select( candidate => ( candidate, cost: costFunction( candidate ) ) )
                      .OrderBy( tuple => tuple.cost );
                      
            var best = candidatesCost.First().candidate;

            return best;
        }
    }
}

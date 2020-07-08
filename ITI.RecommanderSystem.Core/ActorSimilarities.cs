using System.Collections.Generic;
using ITI.RecommanderSystem.Core.DataStructure;

namespace ITI.RecommanderSystem.Core
{
    public static class ActorSimilarities
    {
        private static readonly WeightedResult<int>.WeightedResultAscending ResultAscending =
            new WeightedResult<int>.WeightedResultAscending();

        public static float[ , ] ComputeActorsSimilarities
        (
            int[ , ] data
        )
        {
            var count = data.GetLength( 0 );
            var matrix = new float[ count, count ];

            var lowerBounds = 0;
            for( var i = 0; i < count; ++i )
            {
                for( var j = lowerBounds; j < count; ++j )
                {
                    if( i == j )
                    {
                        matrix[ i, j ] = 1;
                    }
                    else
                    {
                        var pearsonScore = SimilarityComputer.Pearson( data, i, j );
                        matrix[ i, j ] = pearsonScore;
                        matrix[ j, i ] = pearsonScore;
                    }
                }

                lowerBounds += 1;
            }

            return matrix;
        }

        public static IReadOnlyCollection<WeightedResult<int>> GetSimilarActors
        (
            int actorId,
            float[ , ] actorSimilarities,
            int count
        )
        {
            var n = actorSimilarities.GetLength( 0 );
            var actorIdx = actorId - 1;

            var bestKeeper = new BestKeeper<WeightedResult<int>>( count, ResultAscending );

            for( var i = 0; i < n; ++i )
            {
                if( i == actorIdx ) continue;

                var actorSimilarity = new WeightedResult<int>( i + 1, actorSimilarities[ actorIdx, i ] );
                bestKeeper.Add( actorSimilarity );
            }

            return bestKeeper;
        }

        public static IReadOnlyCollection<WeightedResult<int>> GetActorBasedRecommendations
        (
            int actorId,
            int[ , ] ratings,
            IReadOnlyCollection<WeightedResult<int>> similarActors,
            int count
        )
        {
            var elementCount = ratings.GetLength( 1 );
            var actorIdx = actorId - 1;

            var bestKeeper = new BestKeeper<WeightedResult<int>>( count, ResultAscending );

            for( var elementIdx = 0; elementIdx < elementCount; ++elementIdx )
            {
                if( ratings[ actorIdx, elementIdx ] > 0 ) continue;

                var hasSimilarRate = false;

                float weightedSum = 0, similaritySum = 0;
                foreach( var (id, similarity) in similarActors )
                {
                    var otherActorIdx = id - 1;
                    var rate = ratings[ otherActorIdx, elementIdx ];
                    if( rate == 0 ) continue;

                    hasSimilarRate = true;
                    weightedSum += rate * similarity;
                    similaritySum += similarity;
                }

                if( !hasSimilarRate ) continue;

                var score = similaritySum == 0 ? 0F : weightedSum / similaritySum;
                var result = new WeightedResult<int>( elementIdx + 1, score );
                bestKeeper.Add( result );
            }

            return bestKeeper;
        }
    }
}

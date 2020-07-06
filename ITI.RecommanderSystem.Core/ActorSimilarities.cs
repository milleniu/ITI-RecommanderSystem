using System.Collections.Generic;
using ITI.RecommanderSystem.Core.DataStructure;

namespace ITI.RecommanderSystem.Core
{
    public static class ActorSimilarities
    {
        public static float[,] ComputeActorsSimilarities
        (
            int[,] data
        )
        {
            var count = data.GetLength(0);
            var matrix = new float[count, count];

            var lowerBounds = 0;
            for (var i = 0; i < count; ++i)
            {
                for (var j = lowerBounds; j < count; ++j)
                {
                    if (i == j)
                    {
                        matrix[i, j] = 1;
                    }
                    else
                    {
                        var pearsonScore = SimilarityComputer.Pearson(data, i, j);
                        matrix[i, j] = pearsonScore;
                        matrix[j, i] = pearsonScore;
                    }
                }

                lowerBounds += 1;
            }

            return matrix;
        }

        public static IReadOnlyCollection<(int userId, float similarity)> GetSimilarActors
        (
            int actorId,
            float[,] actorSimilarities,
            int count
        )
        {
            var n = actorSimilarities.GetLength(0);
            var actorIdx = actorId - 1;

            var bestKeeper = new BestKeeper<(int userId, float similarity)>
            (
                count,
                (a, b) => a.similarity > b.similarity ? 1 : -1
            );

            for (var i = 0; i < n; ++i)
            {
                if (i == actorIdx) continue;
                bestKeeper.Add((i + 1, actorSimilarities[actorIdx, i]));
            }

            return bestKeeper;
        }

        public static IReadOnlyCollection<(int elementId, float score)> GetActorBasedRecommendations
        (
            int actorId,
            int[,] ratings,
            IReadOnlyCollection<(int userId, float similarity)> similarActors,
            int count
        )
        {
            var elementCount = ratings.GetLength(1);
            var actorIdx = actorId - 1;

            var bestKeeper = new BestKeeper<(int elementId, float score)>
            (
                count,
                (a, b) => a.score > b.score ? 1 : -1
            );

            for (var elementIdx = 0; elementIdx < elementCount; ++elementIdx)
            {
                if (ratings[actorIdx, elementIdx] > 0) continue;

                var hasSimilarRate = false;

                float weightedSum = 0, similaritySum = 0;
                foreach (var (id, similarity) in similarActors)
                {
                    var otherActorIdx = id - 1;
                    var rate = ratings[otherActorIdx, elementIdx];
                    if (rate == 0) continue;

                    hasSimilarRate = true;
                    weightedSum += rate * similarity;
                    similaritySum += similarity;
                }

                if (!hasSimilarRate) continue;

                var score = similaritySum == 0 ? 0F : weightedSum / similaritySum;
                bestKeeper.Add((elementIdx + 1, score));
            }

            return bestKeeper;
        }
    }
}

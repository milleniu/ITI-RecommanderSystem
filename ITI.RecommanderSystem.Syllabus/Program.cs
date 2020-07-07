using System;
using ITI.RecommanderSystem.Core;

namespace ITI.RecommanderSystem.Syllabus
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var ratings = new[,]
            {
                { 4, 3, 0, 0, 5, 0 },
                { 5, 0, 4, 0, 4, 0 },
                { 4, 0, 5, 3, 4, 0 },
                { 0, 3, 0, 0, 0, 5 },
                { 0, 4, 0, 0, 0, 4 },
                { 0, 0, 2, 4, 0, 5 }
            };

            const int userId = 1;
            var usersSimilarities = ActorSimilarities.ComputeActorsSimilarities(ratings);
            var similarUsers = ActorSimilarities.GetSimilarActors(userId, usersSimilarities, 2);
            var recommendations = ActorSimilarities.GetActorBasedRecommendations(userId, ratings, similarUsers, 3);

            foreach (var (id, score) in recommendations)
                Console.WriteLine($"{id}: {score}");
        }
    }
}

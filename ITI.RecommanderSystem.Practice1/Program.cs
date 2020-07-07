using ITI.RecommanderSystem.Core;
using ITI.RecommanderSystem.CSV;
using ITI.RecommanderSystem.Practice1.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ITI.RecommanderSystem.Practice1
{
    internal static class Program
    {
        private static void Main( string[] args )
        {
            var dataFolder = args[ 0 ];
            var userId = short.Parse( args[ 1 ] );
            var recommendationsCount = short.Parse( args[ 2 ] );
            var similarUsersCount = short.Parse( args[ 3 ] );

            var csvConfiguration = CSVLoader.InitializeConfiguration
            (
                "::",
                configuration =>
                {
                    configuration.RegisterClassMap<Movie.MovieMap>();
                    configuration.RegisterClassMap<User.UserMap>();
                    configuration.RegisterClassMap<UserMovieRating.UserMovieRatingMap>();
                }
            );

            var users = CSVLoader.ReadCsv<User>( dataFolder, "users.dat", csvConfiguration ).ToArray();
            var movies = CSVLoader.ReadCsv<Movie>( dataFolder, "movies.dat", csvConfiguration ).ToArray();
            var ratings = CSVLoader.ReadCsv<UserMovieRating>( dataFolder, "ratings.dat", csvConfiguration ).ToArray();

            var usersRatings = GenerateRatingsMatrix( users, movies, ratings );
            var usersSimilarities = ActorSimilarities.ComputeActorsSimilarities( usersRatings );
            var similarUsers = ActorSimilarities.GetSimilarActors( userId, usersSimilarities, similarUsersCount );
            var recommendations = ActorSimilarities.GetActorBasedRecommendations( userId, usersRatings, similarUsers, recommendationsCount );

            foreach( var (movieId, score) in recommendations )
                Console.WriteLine( $"{movies[ movieId - 1 ].Title}: {score}" );
        }

        private static int[ , ] GenerateRatingsMatrix
        (
            IReadOnlyCollection<User> users,
            IReadOnlyCollection<Movie> movies,
            IEnumerable<UserMovieRating> ratings
        )
        {
            var matrix = new int[ users.Count, movies.Count ];

            foreach( var rating in ratings )
                matrix[ rating.UserId - 1, rating.MovieId - 1 ] = rating.Rating;

            return matrix;
        }
    }
}

using System;

namespace ITI.RecommanderSystem.Core
{
    public static class SimilarityComputer
    {
        private static float EuclideanDistance
        (
            int[ , ] matrix,
            int xIdx,
            int yIdx
        )
        {
            var n = matrix.GetLength( 1 );

            float sum = 0;
            for( var i = 0; i < n; ++i )
                sum += ( matrix[ yIdx, i ] - matrix[ xIdx, i ] ) * ( matrix[ yIdx, i ] - matrix[ xIdx, i ] );

            return MathF.Sqrt( sum );
        }

        public static float Euclidean
        (
            int[ , ] matrix,
            int xIdx,
            int yIdx
        )
        {
            return 1F / ( EuclideanDistance( matrix, xIdx, yIdx ) + 1F );
        }

        public static float Cosine
        (
            int[ , ] matrix,
            int xIdx,
            int yIdx
        )
        {
            var n = matrix.GetLength( 1 );

            float xSquaredSum = 0, ySquaredSum = 0, dot = 0;
            for( var i = 0; i < n; ++i )
            {
                xSquaredSum += matrix[ xIdx, i ] * matrix[ xIdx, i ];
                ySquaredSum += matrix[ yIdx, i ] * matrix[ yIdx, i ];
                dot += matrix[ xIdx, i ] * matrix[ yIdx, i ];
            }

            return dot / ( MathF.Sqrt( xSquaredSum ) * MathF.Sqrt( ySquaredSum ) );
        }

        public static float Pearson
        (
            int[ , ] matrix,
            int xIdx,
            int yIdx
        )
        {
            var n = matrix.GetLength( 1 );

            float sum = 0,
                  xSum = 0,
                  xSquaredSum = 0,
                  ySum = 0,
                  ySquaredSum = 0;

            for( var i = 0; i < n; ++i )
            {
                sum += matrix[ xIdx, i ] * matrix[ yIdx, i ];

                xSum += matrix[ xIdx, i ];
                xSquaredSum += matrix[ xIdx, i ] * matrix[ xIdx, i ];

                ySum += matrix[ yIdx, i ];
                ySquaredSum += matrix[ yIdx, i ] * matrix[ yIdx, i ];
            }

            return ( sum - xSum * ySum / n )
                 / MathF.Sqrt( ( xSquaredSum - xSum * xSum / n ) * ( ySquaredSum - ySum * ySum / n ) );
        }
    }
}

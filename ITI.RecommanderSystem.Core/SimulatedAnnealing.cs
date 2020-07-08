using System;
using System.Collections.Generic;
using System.Linq;
using QuickGraph;

namespace ITI.RecommanderSystem.Core
{
    public static class SimulatedAnnealing
    {
        public static (int[] Result, int iterations) ResolveInDomain
        (
            (int, int)[] domain,
            Func<IReadOnlyList<int>, float> costFunction,
            float T = 10_000,
            float TMin = 0.1F,
            float cool = 0.95F,
            int step = 1
        )
        {
            var random = new Random();

            var vector = new int[ domain.Length ];
            var vectorB = new int[ domain.Length ];

            void CopyVector()
                => Array.Copy( vector, vectorB, vector.Length );

            for( var i = 0; i < vector.Length; i++ )
            {
                var (min, max) = domain[ i ];
                vector[ i ] = random.Next( min, max + 1 );
            }

            CopyVector();

            var iterations = 0;
            while ( T > TMin )
            {
                var i = random.Next( 0, domain.Length );
                var direction = random.Next( -step, step + 1 );
                var (min, max) = domain[i];

                CopyVector();
                vectorB[ i ] = Math.Max( Math.Min( max, vectorB[ i ] + direction ), min );

                var energy = costFunction( vector );
                var energyB = costFunction( vectorB );
                var p = Math.Pow( Math.E, ( -energyB - energy ) / T );

                if( energyB < energy || random.NextDouble() < p )
                    Array.Copy( vectorB, vector, vector.Length );

                T *= cool;
                iterations += 1;
            }

            return (vector, iterations);
        }

        public static TVertex[] ResolveGraph<TVertex, TEdge>
        (
            BidirectionalGraph<TVertex, TEdge> graph,
            Func<TVertex[], float> costFunction,
            float T = 10.0F,
            float TMin = 1e-2F,
            float tau = 1e4F
        )
            where TEdge : IEdge<TVertex>
        {
            var random = new Random();

            var T0 = T;
            var path = graph.Vertices.ToArray();
            var EC = costFunction( path );

            var t = 0;
            while( T > TMin )
            {
                var i = random.Next( 0, graph.VertexCount );
                var j = random.Next( 0, graph.VertexCount );

                ( path[ i ], path[ j ] ) = ( path[ j ], path[ i ] );
                var EF = costFunction( path );

                float Metropolis( float E1, float E2 )
                {
                    if( E1 <= E2 )
                    {
                        E2 = E1;
                    }
                    else
                    {
                        var dE = E1 - E2;
                        var p = (float) random.NextDouble();

                        if( p > MathF.Exp( -dE / T ) )
                        {
                            ( path[ i ], path[ j ] ) = ( path[ j ], path[ i ] );
                        }
                        else
                        {
                            E2 = E1;
                        }
                    }

                    return E2;
                }

                EC = Metropolis( EF, EC );

                t += 1;
                T = T0 * MathF.Exp( -t / tau );
            }

            return path;
        }
    }
}

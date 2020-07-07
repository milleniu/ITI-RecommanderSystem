using System;
using System.Collections.Generic;
using System.Linq;
using QuickGraph;

namespace ITI.RecommanderSystem.MarkovChains
{
    public static class SimulatedAnnealing
    {
        public static int[] ResolveInDomain
        (
            (int, int)[] domain,
            Func<IReadOnlyList<int>, float> costFunction,
            float T = 10_000,
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

            while ( T > 0.1F )
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
            }

            return vector;
        }

        public static TVertex[] ResolveGraph<TVertex, TEdge>
        (
            BidirectionalGraph<TVertex, TEdge> graph,
            Func<TVertex[], float> costFunction
        )
            where TEdge : IEdge<TVertex>
        {
            const float T0 = 10.0F;
            const float TMin = 1e-2F;
            const float tau = 1e4F;

            var random = new Random();

            var path = graph.Vertices.ToArray();
            var EC = costFunction( path );

            var t = 0;
            var T = T0;
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

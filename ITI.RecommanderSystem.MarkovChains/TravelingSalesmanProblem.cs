using System;
using System.Linq;
using QuickGraph;

namespace ITI.RecommanderSystem.MarkovChains
{
    public static class TravelingSalesmanProblem
    {
        public static (float Cost, TVertex[] Path) Resolve<TVertex, TEdge>
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

            return (Cost: EC, Path: path);
        }
    }
}

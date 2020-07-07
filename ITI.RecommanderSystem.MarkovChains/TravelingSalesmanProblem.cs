using System;
using System.Collections.Generic;
using System.Linq;
using QuickGraph;

namespace ITI.RecommanderSystem.MarkovChains
{
    public static class TravelingSalesmanProblem
    {
        public static IReadOnlyCollection<TVertex> Resolve<TVertex, TEdge>
        (
            BidirectionalGraph<TVertex, TEdge> graph,
            IDictionary<TEdge, int> costs
        )
            where TEdge : IEdge<TVertex>
        {
            const float T0 = 10.0F;
            const float TMin = 1e-2F;
            const float tau = 1e4F;

            var random = new Random();

            int CostFunction( IReadOnlyList<TVertex> path )
            {
                var cost = 0;

                for( var i = 1; i < path.Count; ++i )
                {
                    if( !graph.TryGetEdge( path[ i - 1 ], path[ i ], out var edge )
                     && !graph.TryGetEdge( path[ i ], path[ i - 1 ], out edge ) )
                        throw new InvalidOperationException( "graph is not complete" );

                    if( !costs.TryGetValue( edge, out var edgeCost ) )
                        throw new InvalidOperationException( "edge cost is unknown" );

                    cost += edgeCost;
                }

                return cost;
            }

            var path = graph.Vertices.ToArray();
            var EC = CostFunction( path );

            var t = 0;
            var T = T0;
            while( T > TMin )
            {
                var i = random.Next( 0, graph.VertexCount );
                var j = random.Next( 0, graph.VertexCount );

                ( path[ i ], path[ j ] ) = ( path[ j ], path[ i ] );
                var EF = CostFunction( path );

                int Metropolis( int E1, int E2 )
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

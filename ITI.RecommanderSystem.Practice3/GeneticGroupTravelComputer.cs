using ITI.RecommanderSystem.Core;
using ITI.RecommanderSystem.Practice2.Model;

namespace ITI.RecommanderSystem.Practice3
{
    public static class GeneticGroupTravelComputer
    {
        public static (int[] Result, float Cost) Compute
        (
            GroupTravel groupTravel,
            int populationSize = 50,
            int iterations = 100
        )
        {
            var domain = new (int, int)[ groupTravel.People.Count * 2 ];
            for( var i = 0; i < domain.Length; i++ )
                domain[ i ] = ( 0, 9 );

            var result = GeneticOptimization.ResolveInDomain
            (
                domain,
                groupTravel.CostFunction,
                populationSize,
                iterations: iterations
            );
            var cost = groupTravel.CostFunction( result );

            return ( result, cost );
        }
    }
}

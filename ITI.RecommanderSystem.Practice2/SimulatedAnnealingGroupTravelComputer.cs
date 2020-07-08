using ITI.RecommanderSystem.Core;
using ITI.RecommanderSystem.Practice2.Model;

namespace ITI.RecommanderSystem.Practice2
{
    public static class SimulatedAnnealingGroupTravelComputer
    {
        public static (int[] Result, float Cost, int iterations) Compute( GroupTravel groupTravel )
        {
            var domain = new (int, int)[groupTravel.People.Count * 2];
            for (var i = 0; i < domain.Length; i++)
                domain[i] = (0, 9);

            var (result, iterations) = SimulatedAnnealing.ResolveInDomain(domain, groupTravel.CostFunction);
            var cost = groupTravel.CostFunction(result);

            return (result, cost, iterations);
        }
    }
}

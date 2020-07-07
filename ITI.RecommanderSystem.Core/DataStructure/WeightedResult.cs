using System.Collections.Generic;

namespace ITI.RecommanderSystem.Core.DataStructure
{
    public readonly struct WeightedResult
    {
        public int ResultId { get; }

        public float Weight { get; }

        public WeightedResult( int resultId, float weight )
        {
            ResultId = resultId;
            Weight = weight;
        }

        public void Deconstruct( out int resultId, out float weight )
        {
            resultId = ResultId;
            weight = Weight;
        }

        public class WeightedResultComparer : IComparer<WeightedResult>
        {
            public int Compare( WeightedResult x, WeightedResult y )
                => x.Weight > y.Weight ? 1 : -1;
        }
    }
}

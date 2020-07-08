using System.Collections.Generic;

namespace ITI.RecommanderSystem.Core.DataStructure
{
    public readonly struct WeightedResult<T>
    {
        public T Result { get; }

        public float Weight { get; }

        public WeightedResult( T result, float weight )
        {
            Result = result;
            Weight = weight;
        }

        public void Deconstruct( out T result, out float weight )
        {
            result = Result;
            weight = Weight;
        }

        public class WeightedResultAscending : IComparer<WeightedResult<T>>
        {
            public int Compare( WeightedResult<T> x, WeightedResult<T> y )
                => x.Weight > y.Weight ? 1 : -1;
        }

        public class WeightedResultDescending : IComparer<WeightedResult<T>>
        {
            public int Compare(WeightedResult<T> x, WeightedResult<T> y)
                => x.Weight > y.Weight ? -1 : 1;
        }
    }
}

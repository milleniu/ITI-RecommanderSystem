using System;
using System.Collections.Generic;

namespace ITI.RecommanderSystem.Practice2.Models
{
    public readonly struct FlightSchedule
    {
        public TimeSpan Departure { get; }

        public TimeSpan Arrival { get; }

        public int Price { get; }

        public FlightSchedule( TimeSpan departure, TimeSpan arrival, int price )
        {
            Departure = departure;
            Arrival = arrival;
            Price = price;
        }

        public sealed class Comparer : IEqualityComparer<FlightSchedule>
        {
            public bool Equals( FlightSchedule x, FlightSchedule y )
                => x.Departure == y.Departure
                && x.Arrival == y.Arrival
                && x.Price == y.Price;

            public int GetHashCode( FlightSchedule obj )
                => HashCode.Combine( obj.Departure, obj.Arrival, obj.Price );
        }
    }
}

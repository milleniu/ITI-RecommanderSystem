using System;
using System.Collections.Generic;

namespace ITI.RecommanderSystem.Practice2.Models
{
    public readonly struct FlightPath
    {
        public string Origin { get; }

        public string Destination { get; }

        public FlightPath( string origin, string destination )
        {
            Origin = origin ?? throw new ArgumentNullException( nameof( origin ) );
            Destination = destination ?? throw new ArgumentNullException( nameof( destination ) );
        }

        public sealed class Comparer : IEqualityComparer<FlightPath>
        {
            public bool Equals( FlightPath x, FlightPath y )
                => x.Origin == y.Origin && x.Destination == y.Destination;

            public int GetHashCode( FlightPath obj )
                => HashCode.Combine( obj.Origin, obj.Destination );
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using ITI.RecommanderSystem.Practice2.DataModels;

namespace ITI.RecommanderSystem.Practice2.Model
{
    public class GroupTravel
    {
        public IReadOnlyList<(string Name, string Depertature)> People { get; }
        public IDictionary<string, Airport> Airports { get; }
        public IDictionary<FlightPath, IList<FlightSchedule>> Flights { get; }
        public string Destination { get; }

        public GroupTravel
        (
            IReadOnlyList<(string Name, string Depertature)> people,
            IDictionary<string, Airport> airports,
            IDictionary<FlightPath, IList<FlightSchedule>> flights,
            string destination
        )
        {
            People = people ?? throw new ArgumentNullException( nameof( people ) );
            Airports = airports ?? throw new ArgumentNullException( nameof( airports ) );
            Flights = flights ?? throw new ArgumentNullException( nameof( flights ) );
            Destination = destination ?? throw new ArgumentNullException( nameof( destination ) );
        }

        public void PrintSchedule
        (
            IReadOnlyList<int> result
        )
        {
            Debug.Assert( result.Count % 2 == 0 );

            for( var i = 0; i < result.Count / 2; ++i )
            {
                var (name, origin) = People[ i ];

                var outFlight = Flights[ new FlightPath( origin, Destination ) ][ result[ i ] ];
                var returnFlight = Flights[ new FlightPath( Destination, origin ) ][ result[ i + 1 ] ];

                Console.WriteLine
                (
                    $"{name,10} {Airports[ origin ].City.Split( '/' )[ 0 ],10} "
                  + $"{outFlight.Departure:hh\\:mm}-{outFlight.Arrival:hh\\:mm} ${outFlight.Price,-3} "
                  + $"{returnFlight.Departure:hh\\:mm}-{returnFlight.Arrival:hh\\:mm} ${returnFlight.Price,-3}"
                );
            }
        }

        public float CostFunction
        (
            IReadOnlyList<int> result
        )
        {
            var totalPrice = 0F;
            var latestArrival = 0;
            var earliestDeparture = 24 * 60;

            for( var i = 0; i < result.Count / 2; ++i )
            {
                var (_, origin) = People[ i ];

                var outFlight = Flights[ new FlightPath( origin, Destination ) ][ result[ i ] ];
                var returnFlight = Flights[ new FlightPath( Destination, origin ) ][ result[ i + 1 ] ];

                totalPrice += outFlight.Price + returnFlight.Price;

                if( latestArrival < outFlight.Arrival.Minutes )
                    latestArrival = outFlight.Arrival.Minutes;

                if( earliestDeparture > returnFlight.Departure.Minutes )
                    earliestDeparture = returnFlight.Departure.Minutes;
            }

            var totalWait = 0;
            for( var i = 0; i < result.Count / 2; ++i )
            {
                var (_, origin) = People[ i ];
                var outFlight = Flights[ new FlightPath( origin, Destination ) ][ result[ i ] ];
                var returnFlight = Flights[ new FlightPath( Destination, origin ) ][ result[ i + 1 ] ];

                totalWait += latestArrival - outFlight.Arrival.Minutes;
                totalWait += returnFlight.Departure.Minutes - earliestDeparture;
            }

            if( latestArrival > earliestDeparture ) totalPrice += 50;

            return totalPrice + totalWait;
        }
    }
}

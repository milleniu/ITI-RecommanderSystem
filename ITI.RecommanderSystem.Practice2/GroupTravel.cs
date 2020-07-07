using System;
using System.Collections.Generic;
using System.Diagnostics;
using ITI.RecommanderSystem.Practice2.Models;
using ITI.RecommanderSystem.MarkovChains;

namespace ITI.RecommanderSystem.Practice2
{
    public class GroupTravel
    {
        private readonly IReadOnlyList<(string Name, string Depertature)> _people;
        private readonly IDictionary<string, Airport> _airports;
        private readonly IDictionary<FlightPath, IList<FlightSchedule>> _flights;
        private readonly string _destination;

        public GroupTravel
        (
            IReadOnlyList<(string Name, string Depertature)> people,
            IDictionary<string, Airport> airports,
            IDictionary<FlightPath, IList<FlightSchedule>> flights,
            string destination
        )
        {
            _people = people ?? throw new ArgumentNullException( nameof( people ) );
            _airports = airports ?? throw new ArgumentNullException( nameof( airports ) );
            _flights = flights ?? throw new ArgumentNullException( nameof( flights ) );
            _destination = destination ?? throw new ArgumentNullException( nameof( destination ) );
        }

        public int[] Compute()
        {
            var domain = new (int, int)[ _people.Count * 2 ];
            for( var i = 0; i < domain.Length; i++ )
                domain[ i ] = ( 0, 9 );

            var result = SimulatedAnnealing.ResolveInDomain( domain, CostFunction );
            var cost = CostFunction( result );
            Console.WriteLine( cost );
            PrintSchedule( result );

            return result;
        }

        public void PrintSchedule
        (
            IReadOnlyList<int> result
        )
        {
            Debug.Assert( result.Count % 2 == 0 );

            for( var i = 0; i < result.Count / 2; ++i )
            {
                var (name, origin) = _people[ i ];

                var outFlight = _flights[ new FlightPath( origin, _destination ) ][ result[ i ] ];
                var returnFlight = _flights[ new FlightPath( _destination, origin ) ][ result[ i + 1 ] ];

                Console.WriteLine
                (
                    $"{name,10} {_airports[ origin ].City.Split( '/' )[ 0 ],10} "
                  + $"{outFlight.Departure:hh\\:mm}-{outFlight.Arrival:hh\\:mm} ${outFlight.Price,-3} "
                  + $"{returnFlight.Departure:hh\\:mm}-{returnFlight.Arrival:hh\\:mm} ${returnFlight.Price,-3}"
                );
            }
        }

        private float CostFunction
        (
            IReadOnlyList<int> result
        )
        {
            var totalPrice = 0F;
            var latestArrival = 0;
            var earliestDeparture = 24 * 60;

            for( var i = 0; i < result.Count / 2; ++i )
            {
                var (_, origin) = _people[ i ];

                var outFlight = _flights[ new FlightPath( origin, _destination ) ][ result[ i ] ];
                var returnFlight = _flights[ new FlightPath( _destination, origin ) ][ result[ i + 1 ] ];

                totalPrice += outFlight.Price + returnFlight.Price;

                if( latestArrival < outFlight.Arrival.Minutes )
                    latestArrival = outFlight.Arrival.Minutes;

                if( earliestDeparture > returnFlight.Departure.Minutes )
                    earliestDeparture = returnFlight.Departure.Minutes;
            }

            var totalWait = 0;
            for( var i = 0; i < result.Count / 2; ++i )
            {
                var (_, origin) = _people[ i ];
                var outFlight = _flights[ new FlightPath( origin, _destination ) ][ result[ i ] ];
                var returnFlight = _flights[ new FlightPath( _destination, origin ) ][ result[ i + 1 ] ];

                totalWait += latestArrival - outFlight.Arrival.Minutes;
                totalWait += returnFlight.Departure.Minutes - earliestDeparture;
            }

            if( latestArrival > earliestDeparture ) totalPrice += 50;

            return totalPrice + totalWait;
        }
    }
}

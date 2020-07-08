using System.Collections.Generic;
using System.Linq;
using CsvHelper.Configuration;
using ITI.RecommanderSystem.CSV;

namespace ITI.RecommanderSystem.Practice2.DataModels
{
    public class Airport
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string StateName { get; set; }
        public string CountryCode { get; set; }

        public sealed class AirportMap : ClassMap<Airport>
        {
            public AirportMap()
            {
                Map( a => a.Code ).Index( 0 );
                Map( a => a.Name ).Index( 1 );
                Map( a => a.City ).Index( 2 );
                Map( a => a.StateName ).Index( 3 );
                Map( a => a.CountryCode ).Index( 4 );
            }
        }

        public static IDictionary<string, Airport> GetAirports(string dataFolder)
        {
            var airportsCSVConfiguration = CSVLoader.InitializeConfiguration
            (
                "|",
                configuration => configuration.RegisterClassMap<Airport.AirportMap>()
            );
            var airports = CSVLoader.ReadCsv<Airport>
            (
                dataFolder,
                "airports.txt",
                airportsCSVConfiguration
            ).ToArray();

            var airportsByCode = airports.Select(a => new KeyValuePair<string, Airport>(a.Code, a));
            return new Dictionary<string, Airport>(airportsByCode);
        }
    }
}

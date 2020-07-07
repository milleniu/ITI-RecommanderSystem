using CsvHelper.Configuration;

namespace ITI.RecommanderSystem.Practice2.Models
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
    }
}

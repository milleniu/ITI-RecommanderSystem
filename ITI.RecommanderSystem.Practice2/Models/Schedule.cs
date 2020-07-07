using System;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace ITI.RecommanderSystem.Practice2.Models
{
    public class Schedule
    {
        public string Departure { get; set; }
        public string Arrival { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public TimeSpan ArrivalTime { get; set; }
        public int Price { get; set; }

        public sealed class ScheduleMap : ClassMap<Schedule>
        {
            public ScheduleMap()
            {
                Map( s => s.Departure ).Index( 0 );
                Map( s => s.Arrival ).Index( 1 );
                Map( s => s.DepartureTime ).Index( 2 ).TypeConverter<TimeSpanConverter>();
                Map( s => s.ArrivalTime ).Index( 3 ).TypeConverter<TimeSpanConverter>();
                Map( s => s.Price ).Index( 4 );
            }

            // ReSharper disable once ClassNeverInstantiated.Local
            private class TimeSpanConverter : DefaultTypeConverter
            {
                public override object ConvertFromString( string text, IReaderRow row, MemberMapData memberMapData )
                    => TimeSpan.Parse( text );
            }
        }
    }
}

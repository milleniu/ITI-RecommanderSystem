using System;
using System.Collections.Generic;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using ITI.RecommanderSystem.CSV;
using ITI.RecommanderSystem.Practice2.Model;

namespace ITI.RecommanderSystem.Practice2.DataModels
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

        public static IDictionary<FlightPath, IList<FlightSchedule>> GetSchedules(string dataFolder)
        {
            var flights = new Dictionary<FlightPath, IList<FlightSchedule>>(new FlightPath.Comparer());

            var scheduleCSVConfiguration = CSVLoader.InitializeConfiguration
            (
                ",",
                configuration => configuration.RegisterClassMap<Schedule.ScheduleMap>()
            );
            var schedules = CSVLoader.ReadCsv<Schedule>
            (
                dataFolder,
                "small-schedule.txt",
                scheduleCSVConfiguration
            ).ToArray();

            foreach (var schedule in schedules)
            {
                var flightPath = new FlightPath(schedule.Departure, schedule.Arrival);
                var flightSchedule = new FlightSchedule(schedule.DepartureTime, schedule.ArrivalTime, schedule.Price);

                if (flights.TryGetValue(flightPath, out var flightPathSchedules))
                    flightPathSchedules.Add(flightSchedule);
                else
                    flights[flightPath] = new List<FlightSchedule> { flightSchedule };
            }

            return flights;
        }
    }
}

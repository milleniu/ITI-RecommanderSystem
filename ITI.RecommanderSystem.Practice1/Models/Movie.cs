using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace ITI.RecommanderSystem.Practice1.Models
{
    public class Movie
    {
        public short MovieId { get; set; }
        public string Title { get; set; }
        public string[] Genres { get; set; }

        public sealed class MovieMap : ClassMap<Movie>
        {
            public MovieMap()
            {
                Map(m => m.MovieId).Index(0);
                Map(m => m.Title).Index(1);
                Map(m => m.Genres).Index( 2 ).TypeConverter<CategoryConverter>();
            }

            // ReSharper disable once ClassNeverInstantiated.Local
            private class CategoryConverter : DefaultTypeConverter
            {
                public override object ConvertFromString( string text, IReaderRow row, MemberMapData memberMapData )
                    => text.Split( '|' );

                public override string ConvertToString( object value, IWriterRow row, MemberMapData memberMapData )
                    => string.Join( '|', (string[]) value );
            }
        }
    }
}

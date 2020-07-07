using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;

namespace ITI.RecommanderSystem.CSV
{
    public static class CSVLoader
    {
        public static CsvConfiguration InitializeConfiguration( string delimiter, Action<CsvConfiguration> configuration )
        {
            var csvConfiguration = new CsvConfiguration( CultureInfo.InvariantCulture )
            {
                Delimiter = delimiter,
                TrimOptions = TrimOptions.Trim,
                HasHeaderRecord = false
            };

            configuration( csvConfiguration );
            return csvConfiguration;
        }

        public static IEnumerable<T> ReadCsv<T>( string dataFolder, string fileName, CsvConfiguration configuration )
        {
            var filePath = Path.Combine( dataFolder, fileName );
            if( !File.Exists( filePath ) ) throw new InvalidOperationException( "File does not exist" );

            using var streamReader = new StreamReader( filePath );
            using var csvReader = new CsvReader( streamReader, configuration);

            return csvReader.GetRecords<T>().ToList();
        }
    }
}

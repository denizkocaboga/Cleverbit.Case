using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Cleverbit.Case.Infrastructure.Csv
{
    public interface ICsvContext
    {
        IEnumerable<T> GetData<T>(string filePath) where T : class;
    }

    public class CsvHelperContext : ICsvContext
    {
        private readonly ILogger<CsvHelperContext> _logger;
        private readonly IFactory _csvFactory;
        private readonly CsvConfiguration _csvConfig;
        private List<string> _badRows;
        private bool _isBadData;

        public CsvHelperContext(ILogger<CsvHelperContext> logger, IFactory factory)
        {

            _logger = logger;
            _csvFactory = factory;
            //ToDo:Inject CsvConfiguration
            _csvConfig = new CsvConfiguration(CultureInfo.CurrentCulture)
            {
                Delimiter = ",",
                //HeaderValidated = null
                IgnoreBlankLines = true,
                HasHeaderRecord = false,
                BadDataFound = context =>
                {
                    _isBadData = true;
                    if (_badRows != null)
                        _badRows.Add(context.RawRecord);

                }
            };
        }

        public IEnumerable<T> GetData<T>(string filePath) where T : class
        {
            using var textReader = new StreamReader(filePath);//ToDo: Find a way for this injection problems?
            using var csv = _csvFactory.CreateReader(textReader, _csvConfig);
            {
                var result = csv.GetRecords<T>().ToArray();

                return result;
            }
        }

        //If csv is a huge file
        public IEnumerable<T[]> GetDataAsChunked<T>(string filePath, int chunkSize, List<string> badRows = null)
        {
            _badRows = badRows;
            List<T> resultChunk = new();

            using var textReader = new StreamReader(filePath);//ToDo: IOC?
            using var csv = _csvFactory.CreateReader(textReader, _csvConfig);

            bool v = csv.Read();
            csv.ReadHeader();

            while (csv.Read())
            {
                T row = csv.GetRecord<T>();

                if (_isBadData)
                {
                    _isBadData = false;
                    continue;
                }

                resultChunk.Add(row);

                if (resultChunk.Count >= chunkSize)
                {
                    yield return resultChunk.ToArray();
                    resultChunk.Clear();
                }

            }
            yield return resultChunk.ToArray();
        }
    }
}

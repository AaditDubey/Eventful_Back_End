using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Text;

namespace Time.Commerce.Application.Extensions
{
    public static class CsvExtension
    {
        public static IList<T> GetRecordsForFile<T>(string filePath, string delimiter = "†")
        {
            using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            using var streamReader = new StreamReader(stream, Encoding.UTF8);

            using var csv = new CsvReader(streamReader, new CsvConfiguration(CultureInfo.CurrentCulture)
            {
                MissingFieldFound = null,
                HasHeaderRecord = true,
                //HeaderValidated =
                //    (isValid, headerNames, _, _) =>
                //    {
                //        if (!isValid)
                //        {
                           
                //        }
                //    },
                Delimiter = delimiter
            });
            var result = csv.GetRecords(typeof(T)).ToList();
            return result.OfType<T>().ToList();
        }
    }
}

namespace Infrastructure
{
    using CsvHelper;
    using Domains.Models;
    using System.Collections.Generic; 
    using System.Globalization;
    using System.IO;

    public class CsvProcessor
    {
        public static List<EmployeeProject> DeserializeFromCsv(string filePath)
        {
            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            return csv.GetRecords<EmployeeProject>().ToList();
        }
    }

}

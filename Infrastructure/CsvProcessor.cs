namespace Infrastructure
{
    using CsvHelper;
    using CsvHelper.Configuration;
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


    public class CsvSafeProcessor
    {
        public static List<EmployeeProject> DeserializeFromCsv(string filePath)
        {
            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            csv.Context.RegisterClassMap<EmployeeProjectMap>();
            return csv.GetRecords<EmployeeProject>().ToList();
        }
    }

    public sealed class EmployeeProjectMap : ClassMap<EmployeeProject>
    {
        public EmployeeProjectMap()
        {
            Map(m => m.EmpID).Index(0);
            Map(m => m.ProjectID).Index(1);
            Map(m => m.DateFrom).Index(2).Convert(args => DateTimeExtensions.ToSafeDateTime(args.Row.GetField(2)));
            Map(m => m.DateTo).Index(3).Convert(args => DateTimeExtensions.ToSafeDateTime(args.Row.GetField(3)));

        }
    }

}

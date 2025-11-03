using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace Infrastructure
{
    public static class DateTimeExtensions
    {
        private static readonly string[] SupportedFormats =
        {
            "yyyy-MM-dd",
            "dd.MM.yyyy",
            "dd/M/yy",
            "MM/d/yy"
        };

        public static DateTime ToSafeDateTime(string? date)
        {
            if (string.IsNullOrWhiteSpace(date) || date.Trim().Equals("NULL", StringComparison.OrdinalIgnoreCase))
                return DateTime.Now;

            if (DateTime.TryParseExact(date.Trim(), SupportedFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
                return result;

            return DateTime.Now;
        }

    }


    public class SafeDateTimeConverter : CsvHelper.TypeConversion.DateTimeConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            return DateTimeExtensions.ToSafeDateTime(text);
        }
    }


}

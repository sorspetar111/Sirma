using CsvHelper.Configuration;

namespace Employers.Net
{
    class EmployeeProjectRecordMap : ClassMap<EmployeeProjectRecord>
    {
        public EmployeeProjectRecordMap()
        {
            Map(m => m.EmpID).Name("EmpID");
            Map(m => m.ProjectID).Name("ProjectID");
            Map(m => m.DateFrom).Name("DateFrom").TypeConverterOption.Format("yyyy-MM-dd");
            Map(m => m.DateTo).Name("DateTo").TypeConverterOption.Format("yyyy-MM-dd");
        }
    }


}

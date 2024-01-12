using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace Employers.Net
{
    public partial class CSVForm : Form
    {

        private List<EmployeeProjectRecord> records;

        public CSVForm()
        {
            InitializeComponent();
            Load();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = "CSV Files|*.csv|All Files|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                records = ReadCsvFile(filePath);
                LoadDataIntoGrid();
            }
        }

        private (int Employee1, int Employee2, int Days) FindLongestWorkingPair(List<EmployeeProjectRecord> records)
        {
            var employeeProjects = records
                .GroupBy(record => record.EmpID)
                .ToDictionary(group => group.Key, group => group.ToList());

            int longestDays = 0;
            (int Employee1, int Employee2, int Days) longestWorkingPair = (0, 0, 0);

            foreach (var employee1 in employeeProjects.Keys)
            {
                foreach (var employee2 in employeeProjects.Keys)
                {
                    if (employee1 != employee2)
                    {
                        var commonProjects = employeeProjects[employee1]
                            .Join(employeeProjects[employee2],
                                  proj1 => proj1.ProjectID,
                                  proj2 => proj2.ProjectID,
                                  (proj1, proj2) => new
                                  {
                                      StartDate = proj1.DateFrom > proj2.DateFrom ? proj1.DateFrom : proj2.DateFrom,
                                      EndDate = proj1.DateTo.HasValue && proj2.DateTo.HasValue
                                                ? (DateTime?)GetMinimalDateTime(proj1.DateTo.Value, proj2.DateTo.Value)
                                                : proj1.DateTo ?? proj2.DateTo
                                  })
                            .Where(proj => proj.StartDate <= proj.EndDate);

                        int commonDays = commonProjects.Sum(proj =>
                        {
                            if (proj.EndDate.HasValue && proj.StartDate.HasValue)
                                return (int)(proj.EndDate.Value - proj.StartDate.Value).TotalDays;
                            return 0;
                        });

                        if (commonDays > longestDays)
                        {
                            longestDays = commonDays;
                            longestWorkingPair = (employee1, employee2, commonDays);
                        }
                    }
                }
            }

            return longestWorkingPair;
        }

        private DateTime GetMinimalDateTime(DateTime date1, DateTime date2)
        {
            return date1 < date2 ? date1 : date2;
        }

        private new void Load()
        {
            // Initialize the DataGridView
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.Columns.Add("EmpID", "Employee ID #1");
            dataGridView1.Columns.Add("EmpID2", "Employee ID #2");
            dataGridView1.Columns.Add("ProjectID", "Project ID");
            dataGridView1.Columns.Add("DaysWorked", "Days Worked");

            // Adjust column properties
            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                col.DataPropertyName = col.Name;
                col.ReadOnly = true;
            }
        }
        private void LoadDataIntoGrid()
        {
            dataGridView1.Rows.Clear();

            foreach (var record in records)
            {
                // Skip records with null DateFrom or DateTo
                if (!record.DateFrom.HasValue || !record.DateTo.HasValue)
                    continue;

                // Calculate days worked
                int daysWorked = (int)(record.DateTo.Value - record.DateFrom.Value).TotalDays;

                // Add a row to the DataGridView
                dataGridView1.Rows.Add(record.EmpID, record.EmpID2, record.ProjectID, daysWorked);
            }
        }

        private List<EmployeeProjectRecord> ReadCsvFile(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                csv.Context.RegisterClassMap<EmployeeProjectRecordMap>();
                return csv.GetRecords<EmployeeProjectRecord>().ToList();
            }
        }
    }


}

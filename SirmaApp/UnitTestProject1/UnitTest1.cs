using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;


[TestClass]
public class YourTestClass
{
    [TestMethod]
    public void FindLongestWorkingPair_Should_Return_Correct_Result()
    {
        // Arrange
        var records = new List<EmployeeProjectRecord>
        {
            new EmployeeProjectRecord { EmpID = 1, ProjectID = 100, DateFrom = new DateTime(2022, 1, 1), DateTo = new DateTime(2022, 1, 5) },
            new EmployeeProjectRecord { EmpID = 1, ProjectID = 101, DateFrom = new DateTime(2022, 1, 3), DateTo = new DateTime(2022, 1, 8) },
            new EmployeeProjectRecord { EmpID = 2, ProjectID = 100, DateFrom = new DateTime(2022, 1, 2), DateTo = new DateTime(2022, 1, 6) },
            new EmployeeProjectRecord { EmpID = 2, ProjectID = 101, DateFrom = new DateTime(2022, 1, 4), DateTo = new DateTime(2022, 1, 7) },
        };

        var expected = (Employee1: 1, Employee2: 2, Days: 4);

        // Act
        var result = FindLongestWorkingPair(records);

        // Assert
        Assert.AreEqual(expected.Employee1, result.Employee1);
        Assert.AreEqual(expected.Employee2, result.Employee2);
        Assert.AreEqual(expected.Days, result.Days);
    }

    [TestMethod]
    public void FindLongestWorkingPair_Should_Handle_Null_Dates()
    {
        // Arrange
        var records = new List<EmployeeProjectRecord>
        {
            new EmployeeProjectRecord { EmpID = 1, ProjectID = 100, DateFrom = null, DateTo = new DateTime(2022, 1, 5) },
            new EmployeeProjectRecord { EmpID = 1, ProjectID = 101, DateFrom = new DateTime(2022, 1, 3), DateTo = null },
            new EmployeeProjectRecord { EmpID = 2, ProjectID = 100, DateFrom = null, DateTo = null },
        };

        var expected = (Employee1: 0, Employee2: 0, Days: 0); // Expecting zeros for null dates

        // Act
        var result = FindLongestWorkingPair(records);

        // Assert
        Assert.AreEqual(expected.Employee1, result.Employee1);
        Assert.AreEqual(expected.Employee2, result.Employee2);
        Assert.AreEqual(expected.Days, result.Days);
    }




    //private (int Employee1, int Employee2, int Days) FindLongestWorkingPair2(List<EmployeeProjectRecord> records)
    //{
    //    var employeeProjects = records
    //        .GroupBy(record => record.EmpID)
    //        .ToDictionary(group => group.Key, group => group.ToList());

    //    int longestDays = 0;
    //    (int Employee1, int Employee2, int Days) longestWorkingPair = (0, 0, 0);

    //    foreach (var employee1 in employeeProjects.Keys)
    //    {
    //        foreach (var employee2 in employeeProjects.Keys)
    //        {
    //            if (employee1 != employee2)
    //            {
    //                var commonProjects = employeeProjects[employee1]
    //                    .Join(employeeProjects[employee2],
    //                          proj1 => proj1.ProjectID,
    //                          proj2 => proj2.ProjectID,
    //                          (proj1, proj2) => new
    //                          {
    //                              StartDate = proj1.DateFrom > proj2.DateFrom ? proj1.DateFrom : proj2.DateFrom,
    //                              EndDate = proj1.DateTo.HasValue && proj2.DateTo.HasValue
    //                                        ? (DateTime?)GetMinimalDateTime(proj1.DateTo.Value, proj2.DateTo.Value)
    //                                        : proj1.DateTo ?? proj2.DateTo
    //                          })
    //                    .Where(proj => proj.StartDate <= proj.EndDate);

    //                int commonDays = commonProjects.Sum(proj => proj.EndDate.HasValue ? (proj.EndDate.Value - proj.StartDate).Days : 0);

    //                if (commonDays > longestDays)
    //                {
    //                    longestDays = commonDays;
    //                    longestWorkingPair = (employee1, employee2, commonDays);
    //                }
    //            }
    //        }
    //    }

    //    return longestWorkingPair;
    //}


    //private List<EmployeeProjectRecord> ReadCsvFile2(string filePath)
    //{
    //    using (var reader = new StreamReader(filePath))
    //    using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
    //    {
    //        return csv.GetRecords<EmployeeProjectRecord>().ToList();
    //    }
    //}




}

namespace Infrastructure
{
    using Domains.Models;
    using System;
    using System.Collections.Generic;

    public class AnalizisisEmployes
    {
        public static List<ProjectAnalysisResult> Analyze(List<EmployeeProject> employeeProjects)
        {
            var employeeProjectDays = employeeProjects.GroupBy(ep => new { ep.ProjectID, ep.EmpID })
                                                      .Select(g => new
                                                      {
                                                          g.Key.ProjectID,
                                                          g.Key.EmpID,
                                                          TotalDaysWorked = g.Sum(ep => ((ep.DateTo ?? DateTime.Now) - ep.DateFrom).Days)
                                                      });


            var topPairsByProject = employeeProjectDays.GroupBy(epd => epd.ProjectID)
                                                       .Select(g => new
                                                       {
                                                           TopTwoEmployees = g.OrderByDescending(emp => emp.TotalDaysWorked).Take(2).ToList()
                                                       })
                                                       .Select(g => new
                                                       {
                                                           ProjectID = g.TopTwoEmployees.First().ProjectID,
                                                           Employee1 = g.TopTwoEmployees.FirstOrDefault()?.EmpID,
                                                           Employee2 = g.TopTwoEmployees.Skip(1).FirstOrDefault()?.EmpID,
                                                           SumOfDaysForTopTwo = g.TopTwoEmployees.Sum(emp => emp.TotalDaysWorked)
                                                       });

            return (List<ProjectAnalysisResult>)topPairsByProject;
        }


        public static (int Employee1, int Employee2, int Days) FindTopLongestWorkingPair(List<EmployeeProject> employeeProjects)
        {
            var topPairsByProject = employeeProjects
                .GroupBy(record => record.EmpID)
                .ToDictionary(group => group.Key, group => group.ToList());

            int longestDays = 0;
            (int Employee1, int Employee2, int Days) longestWorkingPair = (0, 0, 0);

            foreach (var employee1 in topPairsByProject.Keys)
            {
                foreach (var employee2 in topPairsByProject.Keys)
                {
                    if (employee1 != employee2)
                    {
                        var commonProjects = topPairsByProject[employee1]
                            .Join(topPairsByProject[employee2],
                                  proj1 => proj1.ProjectID,
                                  proj2 => proj2.ProjectID,
                                  (proj1, proj2) => new
                                  {
                                      StartDate = proj1.DateFrom > proj2.DateFrom ? proj1.DateFrom : proj2.DateFrom,
                                      EndDate = proj1.DateTo.HasValue && proj2.DateTo.HasValue
                                                ? GetMinimalDateTime(proj1.DateTo.Value, proj2.DateTo.Value)
                                                : proj1.DateTo ?? proj2.DateTo
                                  })
                            .Where(proj => proj.StartDate <= proj.EndDate);

                        int commonDays = commonProjects.Sum(proj =>
                        {
                            if (proj.EndDate.HasValue)
                                return (int)(proj.EndDate.Value - proj.StartDate).TotalDays;
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

        public static DateTime GetMinimalDateTime(DateTime date1, DateTime date2)
        {
            return date1 < date2 ? date1 : date2;
        }


    }
}

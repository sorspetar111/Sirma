using Domains.Models;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/projects")]
    public class ProjectsController : ControllerBase
    {
         
        private readonly ILogger<ProjectsController> _logger;

        public ProjectsController(ILogger<ProjectsController> logger)
        {
            _logger = logger;
        }

        [HttpPost("analyze")]
        public IEnumerable<ProjectAnalysisResult> AnalyzeProjectCsv(IFormFile file)
        {
            // if (file == null || file.Length == 0)
            //    return BadRequest("No file uploaded.");

            using var stream = file.OpenReadStream();
            using var reader = new StreamReader(stream);
            var csvContent = reader.ReadToEnd();

            var employeeProjects = CsvProcessor.DeserializeFromCsv(csvContent);

            // var employeeProjects = new List<EmployeeProject>();
            // var employeeProjects = CsvExample.DeserializeFromCsv("projectsEmployees.csv");
            // List<ProjectAnalysisResult>

            return AnalizisisEmployes.Analyze(employeeProjects);

        }




       
    }
}

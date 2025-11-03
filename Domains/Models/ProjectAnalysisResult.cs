using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.Models
{
    public class ProjectAnalysisResult
    {
        public int ProjectID { get; set; }
        public int? Employee1 { get; set; }
        public int? Employee2 { get; set; }
        public int SumOfDaysForTopTwo { get; set; }
    }

}

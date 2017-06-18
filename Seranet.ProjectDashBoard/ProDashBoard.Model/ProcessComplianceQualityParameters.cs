using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProDashBoard.Models
{
    public class ProcessComplianceQualityParameters
    {
        public int Id { get; set; }
        public string QualityParameter { get; set; }
        public bool Status { get; set; }
        public int ParameterOrder { get; set; }
    }
}
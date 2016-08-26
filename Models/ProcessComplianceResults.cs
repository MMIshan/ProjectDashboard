using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProDashBoard.Models
{
    public class ProcessComplianceResults
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int ProjectId { get; set; }
        public int Year { get; set; }
        public int Quarter { get; set; }
        public int QualityParameterId { get; set; }
        public string QualityParameter { get; set; }
        public string Rating { get; set; }
    }
}
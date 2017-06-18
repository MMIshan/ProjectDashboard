using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProDashBoard.Models
{
    public class ProcessComplianceSummary
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int ProjectId { get; set; }
        public int Year { get; set; }
        public int Quarter { get; set; }
        public double Rating { get; set; }
        public string ProcessVersion { get; set; }
        public double Threshold { get; set; }

    }
}
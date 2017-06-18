using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProDashBoard.Models
{
    public class RiskData
    {
        public string riskTitle { get; set; }
        public string riskImpact { get; set; }
        public string riskProbability { get; set; }
        public double riskValue { get; set; }
        public Project subProject { get; set; }
        public int riskValueSimilarCount { get; set; }
        public string riskUrl { get; set; }

    }
}

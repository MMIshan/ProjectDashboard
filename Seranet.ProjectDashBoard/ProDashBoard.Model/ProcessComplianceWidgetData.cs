using ProDashBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProDashBoard.Model
{
    public class ProcessComplianceWidgetData
    {
        public Project Project { get; set; }
        public double Rating { get; set; }
        public int Year { get; set; }
        public int Quarter { get; set; }
    }
}

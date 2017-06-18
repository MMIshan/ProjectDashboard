using ProDashBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProDashBoard.Model
{
    public class CustomerSatWidgetData
    {
        public Project project { get; set; }
        public double rating { get; set; }
        public int year { get; set; }
        public int quarter { get; set; }
    }
}
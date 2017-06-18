using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProDashBoard.Models
{
    public class Summary
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int Year { get; set; }
        public int Quarter { get; set; }
        public double Rating { get; set; }
    }
}
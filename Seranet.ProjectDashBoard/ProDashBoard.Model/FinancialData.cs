using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProDashBoard.Model
{
    public class FinancialData
    {
        public int EmpId { get; set; }
        public string EmpName { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int AccountId { get; set; }
        public double AllocatedHours { get; set; }
        public double BillableHours { get; set; }
        public double TotalHours { get; set; }
        public int BillingType { get; set; }

    }
}















































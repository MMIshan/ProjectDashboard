using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProDashBoard.Model
{
    public class FinancialExpandedResults
    {
        public int Id { get; set; }
        public int EmpId { get; set; }
        public string EmpName { get; set; }
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Quarter { get; set; }
        public int BillableType { get; set; }
        public double AllocatedHours { get; set; }
        public double BillableHours { get; set; }
        public double TotalReportedHours { get; set; }
        public double ConsiderableHours { get; set; }
        public double ExtraOrLag { get; set; }
        public double CumAllocatedHours { get; set; }
        public double CumBillableHours { get; set; }
        public double CumTotalReportedHours { get; set; }
        public double CumConsiderableHours { get; set; }
    }
}

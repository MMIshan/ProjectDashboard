using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProDashBoard.Model
{
    public class FinancialSummary
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public string MonthName { get; set; }
        public int Quarter { get; set; }
        public double ExpectedHours  { get; set; }
        public double ActualHours  { get; set; }
        public double coveredBillableHours { get; set; }

}
}

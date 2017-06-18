using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProDashBoard.Models
{
    public class CustomerSatisfactionQuestion
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int ProjectId { get; set; }
        public int Year { get; set; }
        public int Quarter { get; set; }
        public int QuestionOrder { get; set; }
        public string ActualQuestion { get; set; }
        public string DashboardQuestion { get; set; }
        public string Answer { get; set; }
        public bool CalcExist { get; set; }
    }
}
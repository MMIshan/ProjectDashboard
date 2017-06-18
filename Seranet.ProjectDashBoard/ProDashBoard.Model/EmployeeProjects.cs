using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProDashBoard.Models
{
    public class EmployeeProjects
    {
        public int Id { get; set; }
        public int EmpId { get; set; }
        public int AccountId { get; set; }
        public int ProjectId { get; set; }
        public bool Availability { get; set; }
        public int BillableHours { get; set; }
        public bool Lead { get; set; }
        public int Billable { get; set; }


    }
}




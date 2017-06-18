using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProDashBoard.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string AccountName { get; set; }
        public string AccCode { get; set; }
        public bool Availability { get; set; }
        public int AccountOwner { get; set; }
        public string Description { get; set; }
        public string AllProjectCodes { get; set; }

    }
}

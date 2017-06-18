using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProDashBoard.Models
{
    public class Spec
    {
        public int id { get; set; }

        public int AccountId { get; set; }

        public string linkId { get; set; }

        public int SpecLevel { get; set; }

        public int PendingCount { get; set; }


    }
}
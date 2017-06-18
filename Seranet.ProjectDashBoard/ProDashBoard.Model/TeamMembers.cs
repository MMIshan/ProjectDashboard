using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProDashBoard.Models
{
    public class TeamMembers
    {
        public int Id { get; set; }
        public string MemberName { get; set; }
        public bool AdminRights { get; set; }
        public bool Availability { get; set; }
    }
}
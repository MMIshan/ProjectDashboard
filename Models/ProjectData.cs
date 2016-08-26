using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProDashBoard.Models
{
    public class ProjectData
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public string Name { get; set; }
        public string ProjetCode { get; set; }
        public bool Enabled { get; set; }
        public byte[] RowVersion { get; set; }
        public string ProjectOwner { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProDashBoard.Models
{
    public class CommonData
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string WikiPageLink { get; set; }
        public string ConfluencePageId { get; set; }
        public string RiskPageUrl { get; set; }


    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProDashBoard.Models
{
    public class SatisfactionResults
    {

        public int Id { get; set; }
        public int MemberId { get; set; }
        public int AccountId { get; set; }
        public int ProjectId { get; set; }
        public int Year { get; set; }
        public int Quarter { get; set; }
        public int QuestionId { get; set; }
        public string Answer { get; set; }
        public string Comment { get; set; }

        
        
    }
}
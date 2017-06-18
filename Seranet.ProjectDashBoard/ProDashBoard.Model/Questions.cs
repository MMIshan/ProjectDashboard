using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProDashBoard.Models
{
    public class Questions
    {
        public int Id { get; set; }
        public string QuestionName { get; set; }
        public string QuestionNote { get; set; }

        public string QuestionHint { get; set; }
        public string QuestionType { get; set; }
        public bool Availability { get; set; }
        public bool CalcExist { get; set; }
        public int QuestionOrder { get; set; }
        public bool Mandatory { get; set; }
        public bool Comment { get; set; }
        public int MaxValue { get; set; }
    }
}
using ProDashBoard.Data;
using ProDashBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProDashBoard.Api
{
    public class QuestionController : ApiController
    {
        private QuestionRepository repo;

        public QuestionController()
        {
            repo = new QuestionRepository();
        }

        [HttpGet, Route("api/Question")]

        public List<Questions> Get()
        {
            return repo.Get();
        }

        [HttpGet, Route("api/Question/{id}")]

        public Questions Get(int id)
        {
            Questions questions = null;
            if (repo.Get(id) != null) {
                questions = repo.Get(id);
            }
            return questions;
            
        }

    }
}

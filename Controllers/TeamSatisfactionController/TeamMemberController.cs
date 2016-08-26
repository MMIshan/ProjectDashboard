using ProDashBoard.DAL;
using ProDashBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProDashBoard.Controllers
{
    public class TeamMemberController : ApiController
    {
        private TeamMemberRepository repo;

        public TeamMemberController()
        {
            repo = new TeamMemberRepository();
        }

        [HttpGet, Route("api/TeamMembers")]

        public List<TeamMembers> Get()
        {
            return repo.Get();
        }

        [HttpGet, Route("api/TeamMembers/{username}")]
        public TeamMembers getSelectedEmployee(string username) {
            return repo.getSelectedEmployee(username);
        }
    }
}

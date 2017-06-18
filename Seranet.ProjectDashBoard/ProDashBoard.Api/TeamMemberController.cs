using ProDashBoard.Data;
using ProDashBoard.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProDashBoard.Api
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

        [HttpPost, Route("api/TeamMembers/addIfNotExist")]
        public int addIfNotExist([FromBody] List<string> userName)
        {
            foreach (string user in userName) {
                TeamMembers tm= repo.getSelectedEmployee(user);
                Debug.WriteLine("Mem " + user);
                if (tm == null)
                {
                    TeamMembers newTeamMember = new TeamMembers();
                    newTeamMember.MemberName = user;
                    newTeamMember.AdminRights = false;
                    newTeamMember.Availability = true;
                    repo.add(newTeamMember);
                    Debug.WriteLine("entered");
                }
            }

            return 0;
        }
        //api/Account/getInacativeAccounts
    }
}

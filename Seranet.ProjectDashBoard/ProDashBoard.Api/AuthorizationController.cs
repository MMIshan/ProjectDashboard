
using ProDashBoard.Data;
using ProDashBoard.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Web.Http;

namespace ProDashBoard.Api
{
    public class AuthorizationController : ApiController
    {
        TeamMemberRepository teamMemberRepo;
        AuthorizationRepository authRepo;

        public AuthorizationController() {
            teamMemberRepo = new TeamMemberRepository();
            authRepo = new AuthorizationRepository();
        }

        [HttpGet, Route("api/Authorization")]
        public string getUsername() {
            return authRepo.getUsername();
        }

        [HttpGet, Route("api/Authorization/getLoggedInUser")]
        public TeamMembers getLoggedInUser()
        {
            return authRepo.getLoggedInUser();
        }

        [HttpGet, Route("api/Authorization/getAdminOrTeamLeadRights/{accountId}")]
        public string getAdminOrTeamLeadRights(int accountId)
        {
            return (authRepo.getAdminRights()+"-"+authRepo.getTeamLeadRights(accountId));
        }


    }
}

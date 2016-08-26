using ProDashBoard.DAL;
using ProDashBoard.DAL.AuthorizationRepo;
using ProDashBoard.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Web.Http;

namespace ProDashBoard.Controllers.AuthorizationController
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

        

    }
}

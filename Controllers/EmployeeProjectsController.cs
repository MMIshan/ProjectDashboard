using ProDashBoard.DAL;
using ProDashBoard.DAL.AuthorizationRepo;
using ProDashBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProDashBoard.Controllers
{
    public class EmployeeProjectsController : ApiController
    {

        private EmployeeProjectsRepository repo;
        private AuthorizationRepository authRepo;
        private AccountRepository accountRepo;
        private TeamMemberRepository teamMemberRepo;

        public EmployeeProjectsController()
        {
            repo = new EmployeeProjectsRepository();
            authRepo = new AuthorizationRepository();
            accountRepo = new AccountRepository();
            teamMemberRepo = new TeamMemberRepository();
        }

        [HttpGet, Route("api/EmployeeProjects/getEmployeeAccounts/{id}")]
        public List<Object[]> getEmployeeAccounts(int id)
        {
            bool authenticate = authRepo.getAdminRights();
            List<Object[]> returnData = new List<object[]>();
            if (authenticate)
            {
                returnData=accountRepo.getAllAccounts();
            }
            else {
                returnData=repo.getSelectedEmployeeAccounts(teamMemberRepo.getSelectedEmployee(authRepo.getUsername()).Id);
            }
            return returnData;
        }

        [HttpGet, Route("api/EmployeeProjects/getEmployeeAccountProjects/{accountId}/{empId}")]
        public List<Object[]> getEmployeeAccountProjects(int accountId,int empId)
        {
            return repo.getSelectedEmployeeAccountProjects(accountId,empId);
        }

        [HttpGet, Route("api/EmployeeProjects/getEmployeeProjects/{empId}")]
        public List<EmployeeProjects> getEmployeeProjects(int empId) {
            return repo.getEmployeeProjects(empId);
        }
    }
}

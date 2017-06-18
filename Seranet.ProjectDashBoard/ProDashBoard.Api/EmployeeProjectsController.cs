using ProDashBoard.Data;
using ProDashBoard.Model;
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
            bool authorized = authRepo.getAdminRights();
            List<Object[]> returnData = new List<object[]>();
            if (authorized)
            {
                returnData = accountRepo.getAllAccounts();
            }
            else
            {
                returnData =repo.getSelectedEmployeeAccounts(teamMemberRepo.getSelectedEmployee(authRepo.getUsername()).Id);
            }
            return returnData;
        }


        [HttpGet, Route("api/EmployeeProjects/getEmployeeAccountsgetEmployeeAccountsTeamSatisfaction/{id}")]
        public List<Object[]> getEmployeeAccountsTeamSatisfaction(int id)
        {
            List<Object[]> returnData = new List<object[]>();
            returnData = repo.getSelectedEmployeeAccounts(teamMemberRepo.getSelectedEmployee(authRepo.getUsername()).Id);
            
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

        [HttpGet, Route("api/EmployeeProjects/getEmployeesOfSelectedProject/{projectId}")]
        public List<EmployeeProjectsData> getEmployeesOfSelectedProject(int projectId) {
            return repo.getEmployeesOfSelectedProject(projectId);
        }

        [HttpPost, Route("api/EmployeeProjects/add")]
        public int add([FromBody] List<EmployeeProjects> employeeProjectList)
        {

            foreach (EmployeeProjects empProject in employeeProjectList)
            {
                repo.add(empProject);
                Debug.WriteLine("List " + empProject.Id + " " + empProject.BillableHours);
            }
            return 1;
        }


        [HttpPut, Route("api/EmployeeProjects/update")]
        public int update([FromBody] List<EmployeeProjects> employeeProjectList)
        {
            
            foreach (EmployeeProjects empProject in employeeProjectList) {
                repo.update(empProject);
                Debug.WriteLine("List "+empProject.Id+" "+empProject.BillableHours);
            }
            return 1;
        }

        
    }
}

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
    public class TeamSatisfactionEmployeeSummaryController : ApiController
    {
        private TeamSatisfactionEmployeeSummaryRepository repo;
       
        

        public TeamSatisfactionEmployeeSummaryController()
        {
            repo = new TeamSatisfactionEmployeeSummaryRepository();
            
            
        }

        [HttpGet, Route("api/TeamSatisfactionEmployeeSummary")]

        public List<TeamSatisfactionEmployeeSummary> Get()
        {
            return repo.Get();
        }

        [HttpGet, Route("api/TeamSatisfactionEmployeeSummary/{accountId}/{year}/{quarter}")]
        public List<TeamSatisfactionEmployeeSummary> Get(int accountId,int year,int quarter)
        {
            
            return repo.getSelectedQuarterSummary(accountId,year,quarter);
        }

        [HttpGet, Route("api/TeamSatisfactionEmployeeSummary/add/{summary}")]

        public int add(TeamSatisfactionEmployeeSummary summary)
        {
            return repo.add(summary);
        }

        [HttpGet, Route("api/TeamSatisfactionEmployeeSummary/getEmployeeSummaryList/{empId}/{accountId}/{year}")]
        public List<TeamSatisfactionEmployeeSummary> getEmployeeSummaryList(int empId,int accountId,int year) {
        return repo.getEmployeeSummaryList(empId,accountId,year);
        }
    }
}

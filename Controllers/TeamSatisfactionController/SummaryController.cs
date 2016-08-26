using ProDashBoard.DAL;
using ProDashBoard.DAL.AuthorizationRepo;
using ProDashBoard.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProDashBoard.Controllers
{
    public class SummaryController : ApiController
    {

        private SummaryRepository repo;
        private AuthorizationRepository authRepo;

        public SummaryController()
        {
            repo = new SummaryRepository();
            authRepo = new AuthorizationRepository();
        }

        [HttpGet, Route("api/Summary")]

        public List<Summary> Get()
        {
            return repo.Get();
        }

        [HttpGet, Route("api/Summary/getSelectedProjectSummaries/{id}")]
       
        public List<Summary> getSelectedProjectSummaries(int id)
        {
            bool rights = authRepo.getAdminRights() || authRepo.getTeamLeadRights(id);
            bool b = authRepo.getAccountRights(id);
            Debug.WriteLine("SummaryAuth "+b+" "+ authRepo.getAdminRights()+" "+ authRepo.getTeamLeadRights(id));

            List<Summary> list = new List<Summary>();
            if (rights) {
                list = repo.getSelectedProjectSummaries(id);
            } else if (b) {
                list = repo.getSelectedProjectSummaries(id); ;
            }
                //repo.getSelectedProjectSummaries(id);
            foreach(Summary summmary in list) {
                Console.WriteLine("Val "+summmary.Rating);
            }
            return list;
        }

        [HttpGet, Route("api/Summary/getLatestProjectSummaries/{projectId}/{year}/{quarter}")]

        public Summary getLatestProjectSummary(int projectId,int year,int quarter)
        {
            bool rights = authRepo.getAdminRights() || authRepo.getTeamLeadRights(projectId);
            bool b = authRepo.getAccountRights(projectId);
            Debug.WriteLine("SummaryAuth " + b + " " + authRepo.getAdminRights() + " " + authRepo.getTeamLeadRights(projectId));

            Summary summary = null;
            if (rights)
            {
                if (repo.getLatestProjectSummary(projectId, year, quarter) != null)
                {
                    summary = repo.getLatestProjectSummary(projectId, year, quarter);
                }
            } else if (b) {
                if (repo.getLatestProjectSummary(projectId, year, quarter) != null)
                {
                    summary = repo.getLatestProjectSummary(projectId, year, quarter);
                }
            }
            return summary;
        }
    }
}

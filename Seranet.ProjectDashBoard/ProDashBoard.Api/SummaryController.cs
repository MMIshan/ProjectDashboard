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
       
        public HttpResponseMessage getSelectedProjectSummaries(int id)
        {
            bool rights = authRepo.getAdminRights() || authRepo.getTeamLeadRights(id);
            bool b = authRepo.getAccountRights(id);
            Debug.WriteLine("SummaryAuth "+b+" "+ authRepo.getAdminRights()+" "+ authRepo.getTeamLeadRights(id));

            List<Summary> list = new List<Summary>();
            list = repo.getSelectedProjectSummaries(id);
            if (authRepo.isAuthorized(id)) {
                return Request.CreateResponse(HttpStatusCode.OK, list);
            } else {
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }
                //repo.getSelectedProjectSummaries(id);
            //foreach(Summary summmary in list) {
            //    Console.WriteLine("Val "+summmary.Rating);
            //}
            //return list;
        }

        [HttpGet, Route("api/Summary/getLatestProjectSummaries/{projectId}/{year}/{quarter}")]

        public HttpResponseMessage getLatestProjectSummary(int projectId,int year,int quarter)
        {
            bool rights = authRepo.getAdminRights() || authRepo.getTeamLeadRights(projectId);
            bool b = authRepo.getAccountRights(projectId);
            Debug.WriteLine("SummaryAuth " + b + " " + authRepo.getAdminRights() + " " + authRepo.getTeamLeadRights(projectId));

            Summary summary = null;
            summary = repo.getLatestProjectSummary(projectId, year, quarter);
            if (authRepo.isAuthorized(projectId))
            {
                return Request.CreateResponse(HttpStatusCode.OK, summary);
            } else  {
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }
            
        }
    }
}

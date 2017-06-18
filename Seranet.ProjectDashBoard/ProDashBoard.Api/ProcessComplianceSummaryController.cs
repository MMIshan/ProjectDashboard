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
    [Authorize]
    public class ProcessComplianceSummaryController : ApiController
    {
        private ProcessComplianceSummaryRepository repo;
        private AuthorizationRepository authRepo;

        public ProcessComplianceSummaryController()
        {
            repo = new ProcessComplianceSummaryRepository();
            authRepo = new AuthorizationRepository();
        }

        [HttpGet, Route("api/ProcessCompliance/getSelectedProjectSummaries/{accountId}/{projectId}")]
        public HttpResponseMessage getSelectedProjectSummary(int accountId, int projectId)
        {
            List<ProcessComplianceSummary> returnData = new List<ProcessComplianceSummary>();
            if (authRepo.isAuthorized(accountId))
            {
                returnData = repo.getSelectedProjectSummaries(accountId, projectId);
                return Request.CreateResponse(HttpStatusCode.OK, returnData);
            }
            else {
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }
        }

        [HttpGet, Route("api/ProcessCompliance/getProcessComplianceWidgetDetails/{accountId}")]
        public HttpResponseMessage getProcessComplianceWidgetDetails(int accountId)
        {
            List<ProcessComplianceWidgetData> returnData =null;
            bool rights = authRepo.getAdminRights() || authRepo.getTeamLeadRights(accountId);
            bool b = authRepo.getAccountRights(accountId);
            Debug.WriteLine("ProcessSummaryAuth " + b + " " + authRepo.getAdminRights() + " " + authRepo.getTeamLeadRights(accountId));
            if (authRepo.isAuthorized(accountId)) {
                returnData = repo.getWidgetData(accountId);
                return Request.CreateResponse(HttpStatusCode.OK, returnData);
            } else {
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }
            
        }

        [HttpGet, Route("api/ProcessCompliance/getSelectedProjectDurationSummary/{accountId}/{projectId}/{Year}/{Quarter}")]
        public ProcessComplianceSummary getSelectedProjectDurationSummary(int accountId,int projectId,int Year,int Quarter)
        {
            return repo.getSelectedProjectDurationSummary(accountId,projectId,Year,Quarter);
        }

        [HttpGet, Route("api/ProcessCompliance/checkSummaryAvailabilityForYear/{ProjectId}/{Year}")]
        public List<ProcessComplianceSummary> checkSummaryAvailabilityForYear(int ProjectId, int Year) {

            return repo.checkSummaryAvailabilityForYear(ProjectId,Year);
        }

       
        }
}

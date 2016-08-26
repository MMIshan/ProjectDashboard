using ProDashBoard.DAL.AuthorizationRepo;
using ProDashBoard.DAL.ProcessComplianceRepo;
using ProDashBoard.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProDashBoard.Controllers.ProcessComplianceController
{
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
        public List<ProcessComplianceSummary> getSelectedProjectSummary(int accountId, int projectId)
        {
            List<ProcessComplianceSummary> returnData = new List<ProcessComplianceSummary>();
            if (authRepo.isAuthenticated(accountId))
            {
                returnData=repo.getSelectedProjectSummaries(accountId, projectId);
            }
            return returnData;
        }

        [HttpGet, Route("api/ProcessCompliance/getProcessComplianceWidgetDetails/{accountId}")]
        public Object[] getProcessComplianceWidgetDetails(int accountId)
        {
            Object[] returnData=null;
            bool rights = authRepo.getAdminRights() || authRepo.getTeamLeadRights(accountId);
            bool b = authRepo.getAccountRights(accountId);
            Debug.WriteLine("ProcessSummaryAuth " + b + " " + authRepo.getAdminRights() + " " + authRepo.getTeamLeadRights(accountId));
            if (rights) {
                returnData = repo.getProcessComplianceWidgetDetails(accountId);
            } else if (b) {
                returnData = repo.getProcessComplianceWidgetDetails(accountId);
            }
            return returnData;
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

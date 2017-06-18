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
    public class CustomerSatisfactionSummaryController : ApiController
    {
        private CustomerSatSummaryRepository repo;
        private AuthorizationRepository authRepo;

        public CustomerSatisfactionSummaryController()
        {
            repo = new CustomerSatSummaryRepository();
            authRepo = new AuthorizationRepository();
        }

        [HttpGet, Route("api/CustomerSatisfaction/getSelectedAccountSummaries/{accountId}")]
        public HttpResponseMessage getSelectedAccountSummaries(int accountId)
        {
            List<List<CustomerSatSummary>> returnData = new List<List<CustomerSatSummary>>();
            bool rights = authRepo.getAdminRights() || authRepo.getTeamLeadRights(accountId);
            bool b = authRepo.getAccountRights(accountId);
            Debug.WriteLine("CustomerSumAuth " + b + " " + authRepo.getAdminRights() + " " + authRepo.getTeamLeadRights(accountId));
            if (authRepo.isAuthorized(accountId))
            {
                returnData = repo.getSelectedAccountSummaries(accountId);
                return Request.CreateResponse(HttpStatusCode.OK, returnData);
            }
            else {
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }
            
        }

        [HttpGet, Route("api/CustomerSatisfaction/getSelectedProjectSummary/{accountId}/{projectId}/{year}/{quarter}")]
        public HttpResponseMessage getSelectedProjectSummary(int accountId,int projectId,int year,int quarter)
        {
            CustomerSatSummary returnData = null;

            if (authRepo.isAuthorized(accountId))
            {
                returnData = repo.getSelectedProjectSummary(accountId, projectId, year, quarter);
                return Request.CreateResponse(HttpStatusCode.OK, returnData);
            }
            else {
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }
            
        }

        [HttpGet, Route("api/CustomerSatisfaction/getCustomerSatisfactionWidgetDetails/{accountId}")]
        public HttpResponseMessage getCustomerSatisfactionWidgetDetails(int accountId)
        {
            Object[] returnData=null;
            bool rights = authRepo.getAdminRights() || authRepo.getTeamLeadRights(accountId);
            bool b = authRepo.getAccountRights(accountId);
            Debug.WriteLine("CustomerSumAuth " + b + " " + authRepo.getAdminRights() + " " + authRepo.getTeamLeadRights(accountId));
            if (authRepo.isAuthorized(accountId))
            {
                returnData= repo.getCustomerSatisfactionWidgetDetails(accountId);
                return Request.CreateResponse(HttpStatusCode.OK, returnData);
            }
            else {
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }
            
        }

        [HttpGet, Route("api/CustomerSatisfaction/getCusSatWidgetData/{accountId}")]
        public HttpResponseMessage getCusSatWidgetData(int accountId) {
            if (authRepo.isAuthorized(accountId))
            {
                List<CustomerSatWidgetData> returnData = repo.getWidgetData(accountId);
                return Request.CreateResponse(HttpStatusCode.OK, returnData);
            }
            else {
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }
            
        }
    }
}

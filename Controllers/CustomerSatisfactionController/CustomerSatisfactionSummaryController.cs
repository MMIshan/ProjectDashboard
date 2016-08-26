using ProDashBoard.DAL.AuthorizationRepo;
using ProDashBoard.DAL.CustomerSatisfactionRepo;
using ProDashBoard.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProDashBoard.Controllers.CustomerSatisfactionController
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
        public List<List<CustomerSatSummary>> getSelectedAccountSummaries(int accountId)
        {
            List<List<CustomerSatSummary>> returnData = new List<List<CustomerSatSummary>>();
            bool rights = authRepo.getAdminRights() || authRepo.getTeamLeadRights(accountId);
            bool b = authRepo.getAccountRights(accountId);
            Debug.WriteLine("CustomerSumAuth " + b + " " + authRepo.getAdminRights() + " " + authRepo.getTeamLeadRights(accountId));
            if (rights || b)
            {
                returnData = repo.getSelectedAccountSummaries(accountId);
            }
            return repo.getSelectedAccountSummaries(accountId);
        }

        [HttpGet, Route("api/CustomerSatisfaction/getSelectedProjectSummary/{accountId}/{projectId}/{year}/{quarter}")]
        public CustomerSatSummary getSelectedProjectSummary(int accountId,int projectId,int year,int quarter)
        {
            CustomerSatSummary returnData = null;
            
            if (authRepo.isAuthenticated(accountId)) {
                returnData= repo.getSelectedProjectSummary(accountId, projectId, year, quarter);
            }
            return returnData;
        }

        [HttpGet, Route("api/CustomerSatisfaction/getCustomerSatisfactionWidgetDetails/{accountId}")]
        public Object[] getCustomerSatisfactionWidgetDetails(int accountId)
        {
            Object[] returnData=null;
            bool rights = authRepo.getAdminRights() || authRepo.getTeamLeadRights(accountId);
            bool b = authRepo.getAccountRights(accountId);
            Debug.WriteLine("CustomerSumAuth " + b + " " + authRepo.getAdminRights() + " " + authRepo.getTeamLeadRights(accountId));
            if (rights)
            {
                returnData= repo.getCustomerSatisfactionWidgetDetails(accountId);
            }
            else if (b) {
                returnData= repo.getCustomerSatisfactionWidgetDetails(accountId);
            }
            return returnData;
        }
    }
}

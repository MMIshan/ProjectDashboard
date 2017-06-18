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
    public class CustomerSatisfactionResultsController : ApiController
    {
        private CustomerSatResultsRepository repo;
        private AuthorizationRepository authRepo;

        public CustomerSatisfactionResultsController()
        {
            repo = new CustomerSatResultsRepository();
            authRepo = new AuthorizationRepository();
        }

        [HttpGet, Route("api/CustomerSatisfaction/getSelectedProjectResults/{accountId}/{projectId}/{year}/{quarter}")]
        public HttpResponseMessage getSelectedProjectResults(int accountId,int projectId,int year,int quarter)
        {
            List<CustomerSatisfactionResults> returnData = new List<CustomerSatisfactionResults>();
            if (authRepo.isAuthorized(accountId))
            {
                returnData = repo.getSelectedCustomerSatResults(accountId, projectId, year, quarter);
                return Request.CreateResponse(HttpStatusCode.OK, returnData);
            }
            else {
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }
            
        }
    }
}

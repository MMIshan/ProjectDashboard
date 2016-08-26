using ProDashBoard.DAL.AuthorizationRepo;
using ProDashBoard.DAL.CustomerSatisfactionRepo;
using ProDashBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProDashBoard.Controllers.CustomerSatisfactionController
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
        public List<CustomerSatisfactionResults> getSelectedProjectResults(int accountId,int projectId,int year,int quarter)
        {
            List<CustomerSatisfactionResults> returnData = new List<CustomerSatisfactionResults>();
            if (authRepo.isAuthenticated(accountId)) {
                returnData= repo.getSelectedCustomerSatResults(accountId, projectId, year, quarter);
            }
            return returnData;
        }
    }
}

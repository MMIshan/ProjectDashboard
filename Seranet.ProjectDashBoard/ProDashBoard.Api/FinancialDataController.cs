using ProDashBoard.Data;
using ProDashBoard.Model;
using ProDashBoard.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProDashBoard.Api
{
    public class FinancialDataController : ApiController
    {
        private FinancialDataRepository repo;
        private AuthorizationRepository authRepo;

        public FinancialDataController(){
            repo = new FinancialDataRepository();
            authRepo = new AuthorizationRepository();
        }

        [HttpGet, Route("api/FinancialData/getDataFromTimeReports/{year}/{month}/{accountId}")]
        public HttpResponseMessage getDataFromTimeReports(int year, int month, int accountId) {
            FinancialFinalData returnData = repo.getDataFromTimeReports(year, month, accountId);
            if (authRepo.isAuthorized(accountId))
            {
                return Request.CreateResponse(HttpStatusCode.OK, returnData);
            }
            else {
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }
        }


    }
}

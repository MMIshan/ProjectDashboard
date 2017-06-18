using ProDashBoard.Data;
using ProDashBoard.Model;
using ProDashBoard.Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProDashBoard.Api
{
    public class FinancialSummaryController : ApiController
    {
        private FinancialSummaryRepository repo;
        private AuthorizationRepository authRepo;
        public FinancialSummaryController() {
            repo = new FinancialSummaryRepository();
            authRepo = new AuthorizationRepository();
        }

        [HttpGet, Route("api/FinancialSummary/getFinalMonthSummary/{AccountId}/{Year}/{Quarter}")]

        public FinancialSummary getFinalMonthSummary(int AccountId,int Year,int Quarter) {

            return repo.getSelectedMonthSummary(Year, AccountId, Quarter);
            
        }

        [HttpGet, Route("api/FinancialSummary/getSummaryDataForChart/{AccountId}/{Year}/{Quarter}")]
        public HttpResponseMessage getSummaryDataForChart(int AccountId, int Year, int Quarter) {
            if (authRepo.isAuthorized(AccountId))
            {
                return Request.CreateResponse(HttpStatusCode.OK, repo.getSummaryDataForChart(AccountId, Year, Quarter));
            }
            else {
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }
            
        }

        [HttpGet, Route("api/FinancialSummary/doesSummaryExists/{AccountId}/{Year}/{Month}")]
        public bool doesSummaryExists(int AccountId, int Year, int Month)
        {

            return repo.doesSummaryExists(AccountId, Year, Month);
        }


    }
}

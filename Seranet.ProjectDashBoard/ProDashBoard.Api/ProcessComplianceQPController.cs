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
    public class ProcessComplianceQPController : ApiController
    {
        private ProcessComplianceQPRepository repo;

        public ProcessComplianceQPController()
        {
            repo = new ProcessComplianceQPRepository();
        }

        [HttpGet, Route("api/ProcessCompliance/getQualityParameters")]
        public List<ProcessComplianceQualityParameters> getQualityParameters()
        {
            return repo.get();
        }

    }
}

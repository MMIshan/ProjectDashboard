using ProDashBoard.Data;
using ProDashBoard.Models;
using ProDashBoard.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProDashBoard.Api
{
    public class RiskManagementController : ApiController
    {
        private ProjectRepository projectRepo;
        private RiskManagementRepository riskRepo;
        private AuthorizationRepository authRepo;
        public RiskManagementController() {
            projectRepo = new ProjectRepository();
            riskRepo = new RiskManagementRepository();
            authRepo = new AuthorizationRepository();

        }
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }

        [HttpGet, Route("api/RiskManagement/getTotalRisksForSelectedAccountSubProjects/{id}")]
        public HttpResponseMessage getTotalRisksForSelectedAccount(int id)
        {
            if (authRepo.isAuthorized(id))
            {
                List<Project> subProjects = projectRepo.getSelectedAccountProjects(id);
                List<RiskData> returnData = new List<RiskData>();
                if (subProjects.Count != 0)
                {
                    returnData = riskRepo.getTotalRisksForSelectAccountSubProjects(subProjects);
                }
                return Request.CreateResponse(HttpStatusCode.OK, returnData);
            }
            else {
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }

        }
    }
}


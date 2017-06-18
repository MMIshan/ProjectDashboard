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
    public class SpecController : ApiController
    {
        private SpecRepository specRepo;
        private AuthorizationRepository authRepo;
        private AccountRepository accountRepo;
        public SpecController() {
            specRepo = new SpecRepository();
            authRepo = new AuthorizationRepository();
            accountRepo = new AccountRepository();
        }
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        [HttpGet, Route("api/Spec/getSpecLevel/{accountId}")]
        public HttpResponseMessage getSpecLevel(int accountId)
        {
            Account acc = accountRepo.Get(accountId);
            List<int> returnData = new List<int>();
            returnData.Add(specRepo.getSpecLevel(acc.AccCode));
            returnData.Add(specRepo.getSpecProjectId(acc.AccCode));
            if (authRepo.isAuthorized(accountId))
            {
                return Request.CreateResponse(HttpStatusCode.OK, returnData);
            }
            else {
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }
            
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
    }
}
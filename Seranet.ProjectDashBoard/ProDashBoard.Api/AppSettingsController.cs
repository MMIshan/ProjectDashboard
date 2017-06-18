
using ProDashBoard.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProDashBoard.Api
{
    public class AppSettingsController : ApiController
    {
        private AppSettingsRepository repo;

        public AppSettingsController()
        {
            repo = new AppSettingsRepository();
        }

        [HttpGet, Route("api/AppSettings/getThreshold")]
        public double getThreshold()
        {
            //return 3.4;
            return Convert.ToDouble(repo.getThreshold());
        }

        [HttpGet, Route("api/AppSettings/getSpecLink")]
        public string getSpecLink()
        {
            return Convert.ToString(repo.getSpecLink());
        } 

        [HttpGet, Route("api/AppSettings/getProcessComplianceLink")]
        public string getProcessComplianceLink()
        {
            return Convert.ToString(repo.getProcessComplianceLink());
        }

        [HttpGet, Route("api/AppSettings/getProcessComplianceVersion")]
        public string getProcessComplianceVersion()
        {
            return Convert.ToString(repo.getProcessComplianceVersion());
        }
    }
}

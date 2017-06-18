using ProDashBoard.Data;
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
    public class CommodDataController : ApiController
    {
        private CommonDataRepository repo;

        public CommodDataController()
        {
            repo = new CommonDataRepository();
        }

        [HttpGet, Route("api/CommonData/{projectId}")]

        public CommonData Get(int projectId)
        {
            
            return repo.getSelectedProjectCommonData(projectId);
        }

        [HttpPut, Route("api/CommonData/updateOrAdd")]
        public int update(CommonData commonData) {
            Debug.WriteLine("data " + commonData.ProjectId + " " + commonData.WikiPageLink + " " + commonData.ConfluencePageId);
            return repo.update(commonData);
        }
    }
}

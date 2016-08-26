using ProDashBoard.DAL.CommonDataRepo;
using ProDashBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProDashBoard.Controllers.CommonDataController
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
    }
}

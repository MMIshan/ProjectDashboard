using ProDashBoard.DAL;
using ProDashBoard.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ProDashBoard.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ProjectController : ApiController
    {
        private ProjectRepository repo;

        public ProjectController()
        {
            repo = new ProjectRepository();
        }

        [HttpGet,Route("api/Project")]
        
        public List<Project> Get()
        {
            return repo.Get();
        }

        [HttpGet,Route("api/Project/{id}")]
        public Project Get(int id)
        {
            return repo.Get(id);
        }

        [HttpGet, Route("api/Project/getSpec/{accountId}")]
        public Spec getSpec(int accountId)
        {
            Spec spec = null;
            Debug.WriteLine("gdfgfd" + spec +"gfd");
            if (repo.GetSpec(accountId) != null) {
                
                spec = repo.GetSpec(accountId);
            }

            if (spec != null)
            {
                Debug.WriteLine("Entered");
            }
            else {
                Debug.WriteLine("NotEntered");
            }
            
            return spec;
        }

        [HttpGet, Route("api/Project/getSelectedAccountProjects/{accountId}")]
        public List<Project> getSelectedAccountProjects(int accountId)
        {
            return repo.getSelectedAccountProjects(accountId);
        }

        [HttpGet, Route("api/Project/getProjectData")]
        public List<ProjectData> getProjectData() {

            return repo.getProjectData();
        }
    }
}

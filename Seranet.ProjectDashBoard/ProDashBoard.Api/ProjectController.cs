using ProDashBoard.Data;
using ProDashBoard.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
//using System.Web.Http.Cors;

namespace ProDashBoard.Api
{
    //[EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ProjectController : ApiController
    {
        private ProjectRepository repo;
        private AuthorizationRepository  authRepo;

        public ProjectController()
        {
            repo = new ProjectRepository();
            authRepo = new AuthorizationRepository();
        }

        [HttpGet, Route("api/Project/getAdminPanelActiveProjects")]

        public List<Project> getAdminPanelActiveProjects()
        {
            return repo.getAdminalPanelActiveProjects();
        }

        [HttpGet, Route("api/Project/getAdminPanelInactiveProjects")]

        public List<Project> getAdminPanelInactiveProjects()
        {
            return repo.getAdminalPanelInactiveProjects();
        }

        [HttpGet, Route("api/Project")]

        public List<Project> Get()
        {
            return repo.Get();
        }

        [HttpGet, Route("api/Project/getInactiveProjects")]

        public List<Project> getInactiveProjects()
        {
            return repo.getInactiveProjects();
        }

        [HttpGet, Route("api/Project/{id}")]
        public Project Get(int id)
        {
            return repo.Get(id);
        }

        [HttpGet, Route("api/Project/getSpec/{accountId}")]
        public Spec getSpec(int accountId)
        {
            Spec spec = null;
            Debug.WriteLine("gdfgfd" + spec + "gfd");
            if (repo.GetSpec(accountId) != null)
            {

                spec = repo.GetSpec(accountId);
            }

            if (spec != null)
            {
                Debug.WriteLine("Entered");
            }
            else
            {
                Debug.WriteLine("NotEntered");
            }

            return spec;
        }

        [HttpGet, Route("api/Project/getSelectedAccountProjects/{accountId}")]
        public HttpResponseMessage getSelectedAccountProjects(int accountId)
        {
            if (authRepo.isAuthorized(accountId))
            {
                return Request.CreateResponse(HttpStatusCode.OK, repo.getSelectedAccountProjects(accountId));
            }
            else {
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }
        }

        [HttpGet, Route("api/Project/getSelectedAdminAccountProjects/{accountId}")]
        public HttpResponseMessage getSelectedAdminAccountProjects(int accountId)
        {
            List<Project> returnData= repo.getSelectedAdminAccountProjects(accountId);
            if (authRepo.getAdminRights() ||( authRepo.getTeamLeadRightsForAnyAccount() && authRepo.getTeamLeadRights(accountId)))
            {
                return Request.CreateResponse(HttpStatusCode.OK, returnData);
            }
            else {
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }
            
        }

        [HttpGet, Route("api/Project/getProjectData")]
        public List<ProjectData> getProjectData()
        {

            return repo.getProjectData();
        }

        [HttpPost, Route("api/Project/add")]
        //[ResponseType(typeof(String))]
        public int add([FromBody] string text)
        {
            string[] returnData= text.Split(':');
            string projectName = returnData[0];
            Debug.WriteLine("Project " + text);
            return repo.add(returnData[0], Convert.ToInt32(returnData[1]));
        }

        [HttpPut, Route("api/Project/update")]
        public int put(Project project)
        {
            Debug.WriteLine("Project Put method Accesses");
            return repo.update(project);
        }

        [HttpPut, Route("api/Project/updateFullProject")]
        public int updateFullProject(Project project) {
            return repo.updateFullProject(project);
        }
    }
}

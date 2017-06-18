using ProDashBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProDashBoard.Model.Repository
{
    public interface IProjectRepository
    {
        List<Project> Get();
        Project Get(int id);
        List<Project> getInactiveProjects();

        Spec GetSpec(int projectid);

        List<Project> getSelectedAccountProjects(int accountId);

        List<ProjectData> getProjectData();
        List<Project> getSelectedAdminAccountProjects(int accountId);

        int add(string projectName,int accountId);

        int update(Project project);
        int updateFullProject(Project project);


    }
}

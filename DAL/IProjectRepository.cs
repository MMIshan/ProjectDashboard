using ProDashBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProDashBoard.DAL
{
    interface IProjectRepository
    {
        List<Project> Get();
        Project Get(int id);

        Spec GetSpec(int projectid);

        List<Project> getSelectedAccountProjects(int accountId);

        List<ProjectData> getProjectData();
    }
}

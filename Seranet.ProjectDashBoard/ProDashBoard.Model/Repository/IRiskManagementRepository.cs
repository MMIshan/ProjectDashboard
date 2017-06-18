using ProDashBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProDashBoard.Model.Repository
{
    public interface IRiskManagementRepository
    {
        List<RiskData> getTotalRisksForSelectAccountSubProjects(List<Project> subProjects);
    }
}

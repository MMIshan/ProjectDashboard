using ProDashBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProDashBoard.Model.Repository
{
    public interface ISummaryRepository
    {

        List<Summary> Get();
        List<Summary> getSelectedProjectSummaries(int projectId);

        Summary getLatestProjectSummary(int projectId, int year, int quarter);

        int updateSelectedSummary(int accountId, int year, int quarter, double rating);



    }
}

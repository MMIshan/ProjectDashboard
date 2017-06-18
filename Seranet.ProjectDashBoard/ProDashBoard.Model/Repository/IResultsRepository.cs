
using ProDashBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProDashBoard.Model.Repository
{
    public interface IResultsRepository
    {
        List<SatisfactionResults> Get();
        SatisfactionResults Get(int id);

        List<List<object[]>> getSelectedResults(int projectid, int year, int quarter);

        List<Object[]> getReviweData(int projectId, int year, int quarter, int employeeId);

        int add(SatisfactionResults results);
    }
}

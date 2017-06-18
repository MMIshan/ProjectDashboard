using ProDashBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProDashBoard.Model.Repository
{
    public interface IProcessComplianceResultsRepository
    {
        List<ProcessComplianceResults> getSelectedProjectResults(int AccountId, int ProjectId, int year, int quarter);
        int add(ProcessComplianceResults results);
    }
}

using ProDashBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProDashBoard.Model.Repository
{
    public interface ITeamSatisfactionEmployeeSummaryRepository
    {
        List<TeamSatisfactionEmployeeSummary> Get();
        List<TeamSatisfactionEmployeeSummary> getSelectedQuarterSummary(int accountId,int year,int quarter);

        int add(TeamSatisfactionEmployeeSummary summary);

        double getSelectedAccountSummaryTotal(int accountId, int year, int quarter);
        List<TeamSatisfactionEmployeeSummary> getEmployeeSummaryList(int empId,int accountId, int year);
    }
}

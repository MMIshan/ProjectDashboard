using ProDashBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProDashBoard.DAL.ProcessComplianceRepo
{
    interface IProcessComplianceSummaryRepository
    {
        List<ProcessComplianceSummary> getSelectedProjectSummaries(int AccountId, int ProjectId);

        Object[] getProcessComplianceWidgetDetails(int AccountId);
        ProcessComplianceSummary getSelectedProjectDurationSummary(int AccountId, int ProjectId, int year, int quarter);
        List<ProcessComplianceSummary> checkSummaryAvailabilityForYear(int ProjectId, int year);

        int add(ProcessComplianceSummary summary);
    }
}

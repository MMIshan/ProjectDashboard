using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProDashBoard.Model.Repository
{
    public interface IFinancialSummaryRepository
    {
        void add(FinancialSummary summary);

        FinancialSummary getSelectedMonthSummary(int year,int accountId,int quarter);
        int updatesummary(FinancialSummary summary);
        List<FinancialSummary> getSummaryDataForChart(int accountId, int year, int quarter);
    }
}

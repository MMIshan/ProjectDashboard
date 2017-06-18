using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProDashBoard.Model.Repository
{
    public interface IFinancialResultsRepository
    {
        int add(List<FinancialExpandedResults> results);

    List<List<FinancialExpandedResults>> getSelectedFinancialResults(int year, int quarter, int accountId);

    int getEmployeeTimeReportMaxMonth(int accountId, int year, int quarter, int empId);

    FinancialExpandedResults getIndividualEmpDataForLastMonthInQuarter(int accountId, int empId, int year, int quarter);

    }
}

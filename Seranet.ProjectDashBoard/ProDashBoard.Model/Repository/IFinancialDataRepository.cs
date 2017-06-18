using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProDashBoard.Model.Repository
{
   public interface IFinancialDataRepository
    {
        FinancialFinalData getDataFromTimeReports(int year, int month, int accountId);
    }
}


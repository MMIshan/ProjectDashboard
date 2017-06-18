using ProDashBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProDashBoard.Model.Repository
{
    public interface ICustomerSatSummaryRepository
    {
        List<CustomerSatSummary> Get();
        List<List<CustomerSatSummary>> getSelectedAccountSummaries(int AccountId);

        CustomerSatSummary getSelectedProjectSummary(int AccountId,int ProjectId,int year,int quarter);
        Object[] getCustomerSatisfactionWidgetDetails(int AccountId);

    }
}

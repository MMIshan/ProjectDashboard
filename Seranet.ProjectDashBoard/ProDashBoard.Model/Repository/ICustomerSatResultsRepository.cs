using ProDashBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProDashBoard.Model.Repository
{
    public interface ICustomerSatResultsRepository
    {
        List<CustomerSatisfactionResults> getSelectedCustomerSatResults(int accountId,int projectId,int year,int quarter);
    }
}

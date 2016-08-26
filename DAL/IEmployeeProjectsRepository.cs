using ProDashBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProDashBoard.DAL
{
    interface IEmployeeProjectsRepository
    {
        List<EmployeeProjects> Get();

        List<Object[]> getSelectedEmployeeAccounts(int empId);
        List<Object[]> getSelectedEmployeeAccountProjects(int accountId, int empId);
        List<EmployeeProjects> getEmployeeProjects(int empId);
        EmployeeProjects getLoggedInUserLeadRights(int accountId, int empId, int lead);


    }
}

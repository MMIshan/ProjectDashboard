using ProDashBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProDashBoard.Model.Repository
{
    public interface IEmployeeProjectsRepository
    {
        List<EmployeeProjects> Get();

        List<Object[]> getSelectedEmployeeAccounts(int empId);
        List<Object[]> getSelectedEmployeeAccountProjects(int accountId, int empId);
        List<EmployeeProjects> getEmployeeProjects(int empId);
        EmployeeProjects getLoggedInUserLeadRights(int accountId, int empId, int lead);
        List<EmployeeProjectsData> getEmployeesOfSelectedProject(int projectId);
        List<EmployeeProjectsData> getEmployeesOfSelectedAccount(int accountId);
        int add(EmployeeProjects employeeProject);
        int update(EmployeeProjects employeeProject);
        EmployeeProjects getLoggedInUserLeadRightsForAnyAccount(int empId);

        List<int> getDistinctEmpIdsForAccounts(int accountId, int year, int quarter);

    }
}

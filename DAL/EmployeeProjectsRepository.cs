using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProDashBoard.Models;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;
using System.Diagnostics;

namespace ProDashBoard.DAL
{
    public class EmployeeProjectsRepository : IEmployeeProjectsRepository
    {

        private readonly IDbConnection _db;

        public EmployeeProjectsRepository()
        {
            _db = new SqlConnection(ConfigurationManager.ConnectionStrings["DashBoard1"].ConnectionString);
        }
        public List<EmployeeProjects> Get()
        {
            throw new NotImplementedException();
        }

        public List<Object[]> getSelectedEmployeeAccounts(int empId)
        {
            return this._db.Query("SELECT distinct a.AccountName as name,a.id as accountId FROM EmployeeProjects ea,Project p,Account a WHERE ea.ProjectId=p.Id and p.Enabled=1 and p.AccountId=a.Id and ea.Availability=1 and ea.EmpId='"+empId+"'").Select(d => new object[] { d.name,d.accountId }).ToList();
        }

        public List<Object[]> getSelectedEmployeeAccountProjects(int accountId,int empId)
        {
            return this._db.Query("SELECT p.id,p.Name FROM EmployeeProjects ea,Project p,Account a WHERE ea.ProjectId=p.Id and p.Enabled=1 and p.AccountId=a.Id and ea.Availability=1 and ea.EmpId='" + empId+"' and a.Id='"+accountId+"'").Select(d => new object[] { d.id,d.Name}).ToList();
        }

        public List<EmployeeProjects> getEmployeeProjects(int empId) {
            return this._db.Query<EmployeeProjects>("SELECT * FROM EmployeeProjects ea WHERE ea.Availability=1 and ea.EmpId='" + empId + "'").ToList();
        }

        public EmployeeProjects getLoggedInUserLeadRights(int accountId,int empId,int lead) {

            Debug.WriteLine("emProjects "+accountId+" "+empId+" "+lead);
            EmployeeProjects ep=this._db.Query<EmployeeProjects>("select top(1) * from EmployeeProjects ep where ep.Availability=1 and ep.AccountId='"+accountId+"' and ep.EmpId='"+empId+"' and ep.Lead='"+lead+"'").SingleOrDefault();
            return ep;
        }
    }
}

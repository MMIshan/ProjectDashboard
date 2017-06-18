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
using ProDashBoard.Model.Repository;
using ProDashBoard.Model;

namespace ProDashBoard.Data
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

        public EmployeeProjects getLoggedInUserLeadRightsForAnyAccount(int empId)
        {
            EmployeeProjects ep = this._db.Query<EmployeeProjects>("select top(1) * from EmployeeProjects ep where ep.Availability=1 and ep.EmpId='" + empId + "' and ep.Lead=1").SingleOrDefault();
            return ep;
        }

        public List<EmployeeProjectsData> getEmployeesOfSelectedProject(int projectId)
        {
            return this._db.Query<EmployeeProjectsData>("SELECT ea.Id,ea.EmpId,ea.AccountId,tm.MemberName as EmpName,ea.ProjectId,ea.Availability,ea.BillableHours,ea.Lead,ea.Billable FROM EmployeeProjects ea,TeamMembers tm WHERE ea.EmpId=tm.Id /*and ea.Availability=1*/ and ea.ProjectId='" + projectId + "'").ToList();
        }

        public List<EmployeeProjectsData> getEmployeesOfSelectedAccount(int accountId)
        {
            return this._db.Query<EmployeeProjectsData>("SELECT distinct ea.Id,ea.EmpId,ea.AccountId,tm.MemberName as EmpName,ea.ProjectId,ea.Availability,ea.BillableHours,ea.Lead,ea.Billable FROM EmployeeProjects ea,TeamMembers tm WHERE ea.EmpId=tm.Id and ea.Availability=1 and ea.AccountId='" + accountId + "'").ToList();
        }

        public int add(EmployeeProjects employeeProject)
        {

            int datarows = 0;

            datarows = this._db.Execute(@"INSERT EmployeeProjects([EmpId],[AccountId],[ProjectId],[Availability],[BillableHours],[Lead],[Billable]) values (@EmpId,@AccountId,@ProjectId,@Availability,@BillableHours,@Lead,@Billable)",
                new { EmpId = employeeProject.EmpId, AccountId = employeeProject.AccountId, ProjectId = employeeProject.ProjectId, Availability = employeeProject.Availability, BillableHours = employeeProject.BillableHours, Lead=employeeProject.Lead, Billable=employeeProject.Billable });

            return datarows;
        }

        public int update(EmployeeProjects employeeProject) {
            int datarows = 0;

            datarows = this._db.Execute("UPDATE EmployeeProjects set [EmpId]=@EmpId, [Availability]=@Availability,[BillableHours]=@BillableHours,[Lead]=@Lead,[Billable]=@Billable WHERE [Id]=@Id",
                new { EmpId=employeeProject.EmpId, Availability = employeeProject.Availability, BillableHours = employeeProject.BillableHours, Lead = employeeProject.Lead, Billable = employeeProject.Billable, Id = employeeProject.Id });

            Debug.WriteLine("data " + datarows);
            return datarows;
        }

        public List<EmployeeProjects> getEmployeeProjectsForSelectedAccount(int empId,int accountId)
        {
            return this._db.Query<EmployeeProjects>("SELECT * FROM EmployeeProjects ea WHERE ea.Availability=1 and ea.EmpId='" + empId + "' and ea.AccountId='"+accountId+"'").ToList();
        }

        public List<int> getDistinctEmpIdsForAccounts(int accountId,int year,int quarter) {

            return this._db.Query<int>("SELECT distinct ea.EmpId FROM EmployeeProjects ea  WHERE ea.AccountId='" + accountId + "' and ea.EmpId In (select EmpId from FinancialResults where AccountId="+accountId+" and Year="+year+" and Quarter="+quarter+")").ToList();

        }


        //for permission check
        public int isAnAccountOwner(int accountId, int loggedInUserId)
        {
            Account acc = this._db.Query<Account>("SELECT * FROM [Account] WHERE Id='" + accountId + "' and AccountOwner='" + loggedInUserId + "'").SingleOrDefault();
            if (acc != null)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }

        public int isAnyAccountOwner(int loggedInUserId)
        {
            Account acc = this._db.Query<Account>("SELECT top(1) * FROM [Account] WHERE AccountOwner='" + loggedInUserId + "'").SingleOrDefault();
            if (acc != null)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
    }
}


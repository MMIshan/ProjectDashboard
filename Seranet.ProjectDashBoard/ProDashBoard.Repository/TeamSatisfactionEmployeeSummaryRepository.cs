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

namespace ProDashBoard.Data
{
    public class TeamSatisfactionEmployeeSummaryRepository : ITeamSatisfactionEmployeeSummaryRepository
    {
        private readonly IDbConnection _db;
        private AuthorizationRepository authRepo;

        public TeamSatisfactionEmployeeSummaryRepository()
        {
            _db = new SqlConnection(ConfigurationManager.ConnectionStrings["DashBoard1"].ConnectionString);
            authRepo = new AuthorizationRepository();
        }
        public int add(TeamSatisfactionEmployeeSummary summary)
        {
            int output = this._db.Execute(@"INSERT TeamSatisfactionEmployeeSummary([EmpId],[AccountId],[Year],[Quarter],[Rating]) values (@EmpId,@AccountId,@Year,@Quarter,@Rating)",
                new { EmpId = summary.empId, AccountId = summary.AccountId, Year = summary.Year, Quarter = summary.Quarter, Rating = summary.Rating });
            Debug.WriteLine("TeamSum " + output);
            return output;
        }

        public List<TeamSatisfactionEmployeeSummary> Get()
        {
            return this._db.Query<TeamSatisfactionEmployeeSummary>("SELECT [Id],[EmpId],[AccountId],[Year],[Quarter],[Rating] FROM [TeamSatisfactionEmployeeSummary]").ToList();
        }

        public List<TeamSatisfactionEmployeeSummary> getSelectedQuarterSummary(int accountId, int year, int quarter)
        {
            TeamMembers member = authRepo.getLoggedInUser();
            string query = "";
            if (authRepo.getAdminRights() | authRepo.getTeamLeadRights(accountId))
            {
                query = "SELECT [Id],[EmpId],[AccountId],[Year],[Quarter],[Rating] FROM [TeamSatisfactionEmployeeSummary] where AccountId='" + accountId + "' and Year='" + year + "' and Quarter='" + quarter + "' order by EmpId asc";
            }
            else
            {
                query = "SELECT [Id],[EmpId],[AccountId],[Year],[Quarter],[Rating] FROM [TeamSatisfactionEmployeeSummary] where AccountId='" + accountId + "' and Year='" + year + "' and Quarter='" + quarter + "' and EmpId=" + member.Id + "";
            }
            return this._db.Query<TeamSatisfactionEmployeeSummary>(query).ToList();
        }

        public double getSelectedAccountSummaryTotal(int accountId, int year, int quarter)
        {
            return this._db.Query<double>("select ISNULL(sum(t.Rating)/COUNT(*),0) as summary from TeamSatisfactionEmployeeSummary t where t.AccountId='" + accountId + "' and t.Year='" + year + "' and t.Quarter='" + quarter + "'").SingleOrDefault();
        }

        public List<TeamSatisfactionEmployeeSummary> getEmployeeSummaryList(int empId, int accountId, int year)
        {
            return this._db.Query<TeamSatisfactionEmployeeSummary>("SELECT [Id],[EmpId],[AccountId],[Year],[Quarter],[Rating] FROM [TeamSatisfactionEmployeeSummary] where EmpId='" + empId + "' and  AccountId='" + accountId + "' and Year='" + year + "'").ToList();

        }
    }
}
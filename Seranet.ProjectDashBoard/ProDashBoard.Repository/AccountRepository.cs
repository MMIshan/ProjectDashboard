using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProDashBoard.Models;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;
using ProDashBoard.Model.Repository;
using System.Diagnostics;

namespace ProDashBoard.Data
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IDbConnection _db;
        private AppSettingsRepository set;
        private AuthorizationRepository authRepo;
        
        
        public AccountRepository()
        {
            _db = new SqlConnection(ConfigurationManager.ConnectionStrings["DashBoard1"].ConnectionString);
            set = new AppSettingsRepository();
            authRepo = new AuthorizationRepository();
        }
        public List<Account> Get()
        {
            //set.getThreshold();
            return this._db.Query<Account>("SELECT * FROM [Account] WHERE Availability=1 order by AccountName asc").ToList();
        }

        public List<Account> getAdminPanelActiveAccounts()
        {
            string initialQuery ="";
            //set.getThreshold();
            List<Account> accounts = new List<Account>();
            TeamMembers member=authRepo.getLoggedInUser();
            if (authRepo.getAdminRights())
            {
                initialQuery = "SELECT * FROM [Account] WHERE Availability=1 order by AccountName asc";
                accounts = this._db.Query<Account>(initialQuery).ToList();
            }
            else if (authRepo.getTeamLeadRightsForAnyAccount())
            {
                initialQuery = "SELECT a.* FROM [Account] a WHERE Availability=1 and ((a.Id In(select distinct (e.AccountId) from EmployeeProjects e where e.EmpId="+member.Id+" and e.Lead=1)) or a.AccountOwner="+member.Id+") order by a.AccountName asc";
                accounts = this._db.Query<Account>(initialQuery).ToList();
            }
            
            return accounts;
        }

        public List<Account> getAdminPanelInactiveAccounts()
        {
            string initialQuery = "";
            List<Account> accounts = new List<Account>();
            //set.getThreshold();
            TeamMembers member = authRepo.getLoggedInUser();
            if (authRepo.getAdminRights())
            {
                initialQuery = "SELECT * FROM [Account] WHERE Availability=0 order by AccountName asc";
                accounts= this._db.Query<Account>(initialQuery).ToList();
            }
            else if (authRepo.getTeamLeadRightsForAnyAccount())
            {
                initialQuery = "SELECT a.* FROM [Account] a WHERE Availability=0 and ((a.Id In(select distinct (e.AccountId) from EmployeeProjects e where e.EmpId=" + member.Id + " and e.Lead=1)) or a.AccountOwner=" + member.Id + ") order by a.AccountName asc";
                accounts = this._db.Query<Account>(initialQuery).ToList();
            }
            return accounts;
        }
        public List<Account> getInactiveAccounts()
        {
            //set.getThreshold();
            return this._db.Query<Account>("SELECT * FROM [Account] WHERE Availability=0  order by AccountName asc").ToList();
        }

        public Account Get(int id)
        {
            return this._db.Query<Account>("SELECT * FROM [Account] WHERE  Id='"+id+"'").SingleOrDefault();
        }

        public Account Get(string code)
        {
            return this._db.Query<Account>("SELECT * FROM [Account] WHERE AccCode='" + code + "'").SingleOrDefault();
        }

        public Spec GetSpec(int accountId)
        {
            return _db.Query<Spec>("SELECT [Id],[AccountId],[linkId],[SpecLevel],[PendingCount] FROM [Spec] WHERE AccountId = '" + accountId + "'").SingleOrDefault();
        }

        public List<Object[]> getAllAccounts()
        {
            return this._db.Query("SELECT AccountName,Id FROM Account a WHERE a.Availability=1 order by AccountName asc").Select(d => new object[] { d.AccountName, d.Id }).ToList();
        }

        public int add(Account account)
        {

            int datarows = 0;

            datarows = this._db.Execute(@"INSERT Account([AccountName],[AccCode],[Availability],[AccountOwner],[Description],[AllProjectCodes]) values (@AccountName,@AccCode,@Availability,@AccountOwner,@Description,@AllProjectCodes)",
                new { AccountName = account.AccountName, AccCode = account.AccCode, Availability = account.Availability, AccountOwner = account.AccountOwner, Description = account.Description, AllProjectCodes= account.AllProjectCodes });

            return datarows;
        }

        public int update(Account account)
        {
            int datarows = 0;

            datarows = this._db.Execute("UPDATE Account set [AccountName]=@AccountName,[AccCode]=@AccCode,[Availability]=@Availability,[AccountOwner]=@AccountOwner,[Description]=@Description,[AllProjectCodes]=@AllProjectCodes WHERE [Id]=@Id",
                new { AccountName=account.AccountName, AccCode=account.AccCode, Availability=account.Availability, AccountOwner=account.AccountOwner, Description=account.Description, AllProjectCodes=account.AllProjectCodes, Id=account.Id });

            Debug.WriteLine("data "+datarows);
            return datarows;
        }

        
    }
}

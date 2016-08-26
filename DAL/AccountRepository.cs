using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProDashBoard.Models;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;
using ProDashBoard.DAL.SettingsFileRepo;

namespace ProDashBoard.DAL
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IDbConnection _db;
        private AppSettingsRepository set;
        public AccountRepository()
        {
            _db = new SqlConnection(ConfigurationManager.ConnectionStrings["DashBoard1"].ConnectionString);
            set = new AppSettingsRepository();
        }
        public List<Account> Get()
        {
            set.getThreshold();
            return this._db.Query<Account>("SELECT [Id],[AccountName],[AccCode],[Availability] FROM [Account] WHERE Availability=1").ToList();
        }

        public Account Get(int id)
        {
            return this._db.Query<Account>("SELECT [Id],[AccountName],[AccCode],[Availability] FROM [Account] WHERE Availability=1 and Id='"+id+"'").SingleOrDefault();
        }

        public Spec GetSpec(int accountId)
        {
            return _db.Query<Spec>("SELECT [Id],[AccountId],[linkId],[SpecLevel],[PendingCount] FROM [Spec] WHERE AccountId = '" + accountId + "'").SingleOrDefault();
        }

        public List<Object[]> getAllAccounts()
        {
            return this._db.Query("SELECT AccountName,Id FROM Account a WHERE a.Availability=1").Select(d => new object[] { d.AccountName, d.Id }).ToList();
        }
    }
}

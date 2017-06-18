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

namespace ProDashBoard.Data
{
    public class CustomerSatResultsRepository : ICustomerSatResultsRepository
    {
        private readonly IDbConnection _db;

        public CustomerSatResultsRepository()
        {
            _db = new SqlConnection(ConfigurationManager.ConnectionStrings["DashBoard1"].ConnectionString);
        }
        public List<CustomerSatisfactionResults> getSelectedCustomerSatResults(int accountId, int projectId, int year, int quarter)
        {
            String query = "SELECT [Id],[AccountID],[ProjectID],[Year],[Quarter],[QuestionOrder],[ActualQuestion],[DashboardQuestion],[Answer],[CalcExist] FROM CustomerSatisfactionResults cr where cr.QuestionOrder!=1 and cr.AccountId='" + accountId + "' and cr.ProjectId='" + projectId + "' and Year='" + year + "' and Quarter='" + quarter + "'";
            if (year == 0 & quarter == 0) {
                query = "SELECT [Id],[AccountID],[ProjectID],[Year],[Quarter],[QuestionOrder],[ActualQuestion],[DashboardQuestion],[Answer],[CalcExist] FROM CustomerSatisfactionResults cr where cr.QuestionOrder!=1 and cr.AccountId='" + accountId + "' and cr.ProjectId='" + projectId + "' and Year=(SELECT top(1) [Year] FROM CustomerSatisfactionResults cr1 where cr1.QuestionOrder!=1 and cr1.AccountId=cr.AccountId and cr1.ProjectId=cr.ProjectId order by YEAR desc,Quarter desc) and Quarter=(SELECT top(1) [Quarter] FROM CustomerSatisfactionResults cr2 where cr2.QuestionOrder!=1 and cr2.AccountId=cr.AccountId and cr2.ProjectId=cr.ProjectId order by YEAR desc,Quarter desc)";
            }
            //SELECT [Id],[AccountID],[ProjectID],[Year],[Quarter],[QuestionOrder],[ActualQuestion],[DashboardQuestion],[Answer],[CalcExist] FROM CustomerSatisfactionResults cr where cr.QuestionOrder!=1 and cr.AccountId=1 and cr.ProjectId=1 and Year=(SELECT top(1) [Year] FROM CustomerSatisfactionResults cr1 where cr1.QuestionOrder!=1 and cr1.AccountId=cr.AccountId and cr1.ProjectId=cr.ProjectId order by YEAR desc,Quarter desc) and Quarter=(SELECT top(1) [Quarter] FROM CustomerSatisfactionResults cr2 where cr2.QuestionOrder!=1 and cr2.AccountId=cr.AccountId and cr2.ProjectId=cr.AccountId order by YEAR desc,Quarter desc)
            return this._db.Query<CustomerSatisfactionResults>(query).ToList();
        }
    }
}
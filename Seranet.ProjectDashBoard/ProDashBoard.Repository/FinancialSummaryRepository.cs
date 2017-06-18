
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProDashBoard.Model;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;
using ProDashBoard.Model.Repository;
using System.Diagnostics;

namespace ProDashBoard.Repository
{
    public class FinancialSummaryRepository : IFinancialSummaryRepository
    {
        private readonly IDbConnection _db;
        //private EmailSenderRepository emailSenderRepo;
        public FinancialSummaryRepository() {
            _db = new SqlConnection(ConfigurationManager.ConnectionStrings["DashBoard1"].ConnectionString);
            //emailSenderRepo = new EmailSenderRepository();
        }
        public void add(FinancialSummary summary)
        {
            int datarows = 0;

            datarows = this._db.Execute(@"INSERT FinancialSummary([AccountId],[AccountName],[Year],[Month],[MonthName],[Quarter],[ExpectedHours],[ActualHours],[CoveredBillableHours]) values (@AccountId,@AccountName,@Year,@Month,@MonthName,@Quarter,@ExpectedHours,@ActualHours,@CoveredBillableHours)",
                new { AccountId = summary.AccountId, AccountName = summary.AccountName, Year = summary.Year, Month = summary.Month, MonthName = summary.MonthName, Quarter = summary.Quarter, ExpectedHours = summary.ExpectedHours, ActualHours = summary.ActualHours, CoveredBillableHours=summary.coveredBillableHours });
        }

        public FinancialSummary getSelectedMonthSummary(int year, int accountId, int quarter)
        {
            if (year == 0 & quarter == 0)
            {
                return this._db.Query<FinancialSummary>("SELECT [Id],[AccountId],[AccountName],[Year],[Month],[MonthName],[Quarter],[ExpectedHours],[ActualHours],[CoveredBillableHours] FROM FinancialSummary fs WHERE fs.AccountId=" + accountId + " and fs.Year=(select max(fs1.YEAR) from FinancialSummary fs1 where fs.AccountId=fs1.AccountId) and MONTH=(select top (1) fs2.MONTH from FinancialSummary fs2 where fs.AccountId=fs2.AccountId order by fs2.year desc,fs2.MONTH desc)").SingleOrDefault();
            }
            else
            {
                return this._db.Query<FinancialSummary>("SELECT top(1) [Id],[AccountId],[AccountName],[Year],[Month],[MonthName],[Quarter],[ExpectedHours],[ActualHours],[CoveredBillableHours] FROM FinancialSummary fs WHERE fs.AccountId=" + accountId + " and fs.Year='" + year + "' and fs.Quarter='" + quarter + "' order by Month desc").SingleOrDefault();
            }
            }

        public int updatesummary(FinancialSummary summary)
        {
            int datarows = 0;

            datarows = this._db.Execute("UPDATE FinancialSummary set [ExpectedHours]=@ExpectedHours, [ActualHours]=@ActualHours WHERE [AccountId]=@AccountId and [Year]=@Year and [Month]=@Month",
                new { ExpectedHours = summary.ExpectedHours, ActualHours = summary.ActualHours, AccountId = summary.AccountId, Year = summary.Year, Month = summary.Month });

            return datarows;
        }

        public List<FinancialSummary> getSummaryDataForChart(int accountId, int year, int quarter) {

            string query = "select [Id],[AccountId],[AccountName],[Year],[Month],[MonthName],[Quarter],[ExpectedHours],[CoveredBillableHours] from FinancialSummary fs where fs.[AccountId ]='" + accountId + "' and fs.[Year]=(select top(1) YEAR from FinancialSummary where [AccountId ]=fs.[AccountId ] order by YEAR desc,Quarter desc ) and fs.[Quarter]=(select top(1) Quarter from FinancialSummary where [AccountId ]=fs.[AccountId] order by YEAR desc,Quarter desc)";

            if (year != 0 && quarter != 0) {
                query = "select [Id],[AccountId],[AccountName],[Year],[Month],[MonthName],[Quarter],[ExpectedHours],[CoveredBillableHours] from FinancialSummary fs where fs.[AccountId ]='" + accountId + "' and fs.[Year]='" + year + "' and fs.[Quarter]='" + quarter + "'";
            }
            Debug.WriteLine("SummaryQuery " + query);
            return this._db.Query<FinancialSummary>(query).ToList();
        }

        public bool doesSummaryExists(int accountId,int year,int month) {
            string query = "select * from FinancialSummary where AccountId="+accountId+" and Year="+year+" and Month="+month+"";
            FinancialSummary summary= this._db.Query<FinancialSummary>(query).SingleOrDefault();
            if (summary != null)
            {
                return true;
            }
            else {
                return false;
            }
        }

        public int deleteExistingSummary(int accountId,int year,int quarter,int month)
        {
            return this._db.Execute(@"delete from FinancialSummary where AccountId="+accountId+" and Year="+year+" and Quarter="+quarter+" and Month="+month+" ");
        }

        //select fs.* from FinancialSummary fs where fs.[AccountId ]=1 and fs.[Year]=(select top(1) YEAR from FinancialSummary where [AccountId ]=fs.[AccountId ] order by YEAR desc,Quarter desc ) and fs.[Quarter]=(select top(1) Quarter from FinancialSummary where [AccountId ]=fs.[AccountId] order by YEAR desc,Quarter desc)
    }
}






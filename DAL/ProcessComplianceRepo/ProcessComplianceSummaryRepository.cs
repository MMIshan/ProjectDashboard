using Dapper;
using ProDashBoard.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ProDashBoard.DAL.ProcessComplianceRepo
{
    public class ProcessComplianceSummaryRepository : IProcessComplianceSummaryRepository
    {
        private readonly IDbConnection _db;
        

        public ProcessComplianceSummaryRepository()
        {
            _db = new SqlConnection(ConfigurationManager.ConnectionStrings["DashBoard1"].ConnectionString);
        }

        public List<ProcessComplianceSummary> getSelectedProjectSummaries(int AccountId, int ProjectId)
        {
            return this._db.Query<ProcessComplianceSummary>("SELECT [Id],[AccountId],[ProjectId],[Year],[Quarter],[Rating],[ProcessVersion],[Threshold] FROM [ProcessComplianceSummary] where AccountId='" + AccountId + "' and ProjectId='" + ProjectId + "' order by Year asc,Quarter asc").ToList();
        }
        

        public Object[] getProcessComplianceWidgetDetails(int AccountId)
        {
            Object[] data= null;
            try
            {
                data = this._db.Query("select (select top(1) PERCENT pcs.Year from ProcessComplianceSummary pcs where pcs.AccountId='" + AccountId + "' order by pcs.Year desc,pcs.Quarter desc) as year,(select top(1) PERCENT pcs.Quarter from ProcessComplianceSummary pcs where pcs.AccountId='" + AccountId + "' order by pcs.Year desc,pcs.Quarter desc) as quarter,(sum(pcs.Rating)/(select count(*) from ProcessComplianceSummary pcs1 where pcs1.Year=(select top(1) PERCENT pcs.year from ProcessComplianceSummary pcs order by pcs.Year desc,pcs.Quarter desc) and pcs1.Quarter=(select top(1) PERCENT pcs.Quarter from ProcessComplianceSummary pcs order by pcs.Year desc,pcs.Quarter desc) and pcs1.Rating!=0 and pcs1.AccountId='" + AccountId + "')) as average,((select count(*) from Project p where p.Enabled='" + AccountId + "' and p.AccountId='" + AccountId + "')-(select count(pcs1.Id) from ProcessComplianceSummary pcs1 where pcs1.Year=(select top(1) PERCENT pcs.Year from ProcessComplianceSummary pcs order by pcs.Year desc,pcs.Quarter desc) and pcs1.Quarter=(select top(1) PERCENT pcs.Quarter from ProcessComplianceSummary pcs order by pcs.Year desc,pcs.Quarter desc) and pcs1.Rating!=0 and pcs1.AccountId='" + AccountId + "' )) as difference from ProcessComplianceSummary pcs where pcs.Year=(select top(1) PERCENT pcs.Year from ProcessComplianceSummary pcs where pcs.AccountId='" + AccountId + "' order by pcs.Year desc,pcs.Quarter desc) and pcs.Quarter=(select top(1) PERCENT pcs.Quarter from ProcessComplianceSummary pcs where pcs.AccountId='" + AccountId + "' order by pcs.Year desc,pcs.Quarter desc) and pcs.AccountId='" + AccountId + "'").Select(d => new object[] { d.year, d.quarter, d.average, d.difference }).SingleOrDefault();
               
            }
            catch (Exception e) {

            }
            return data;
        }

        public ProcessComplianceSummary getSelectedProjectDurationSummary(int AccountId, int ProjectId, int year, int quarter)
        {
            return this._db.Query<ProcessComplianceSummary>("SELECT [Id],[AccountId],[ProjectId],[Year],[Quarter],[Rating],[ProcessVersion],[Threshold] FROM [ProcessComplianceSummary] where AccountId='" + AccountId + "' and ProjectId='" + ProjectId + "' and Year="+year+" and Quarter="+quarter+" order by Year asc,Quarter asc").SingleOrDefault();
        }

        public List<ProcessComplianceSummary> checkSummaryAvailabilityForYear(int ProjectId, int year)
        {
            return this._db.Query<ProcessComplianceSummary>("SELECT [Id],[AccountId],[ProjectId],[Year],[Quarter],[Rating],[ProcessVersion],[Threshold] FROM [ProcessComplianceSummary] where ProjectId='" + ProjectId + "' and Year='"+year+"'").ToList();
        }

        public int add(ProcessComplianceSummary summary)
        {

            int datarows = 0;

            datarows = this._db.Execute(@"INSERT ProcessComplianceSummary([AccountID],[ProjectID],[Year],[Quarter],[Rating]) values (@AccountId,@ProjectId,@Year,@Quarter,@Rating)",
                new { AccountId = summary.AccountId, ProjectId = summary.ProjectId, Year = summary.Year, Quarter = summary.Quarter,Rating = summary.Rating });

            return datarows;
        }
    }
}

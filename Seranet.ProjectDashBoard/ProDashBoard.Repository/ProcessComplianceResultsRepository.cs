using Dapper;
using ProDashBoard.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using ProDashBoard.Model.Repository;

namespace ProDashBoard.Data
{
    public class ProcessComplianceResultsRepository : IProcessComplianceResultsRepository
    {
        private readonly IDbConnection _db;

        public ProcessComplianceResultsRepository()
        {
            _db = new SqlConnection(ConfigurationManager.ConnectionStrings["DashBoard1"].ConnectionString);
        }

        public List<ProcessComplianceResults> getSelectedProjectResults(int AccountId, int ProjectId,int year,int quarter)
        {
            StringBuilder builder = new StringBuilder("SELECT pcr.[Id],pcr.[AccountID],pcr.[ProjectID],pcr.[Year],pcr.[Quarter],pcr.[QualityParameterId],pcqr.[QualityParameter],pcr.[Rating] ");
            builder.Append(" FROM [ProcessComplianceResults] pcr,ProcessComplianceQualityParameters pcqr");
            if (year == 0 & quarter == 0)
            {
                builder.Append(" where pcr.QualityParameterId = pcqr.Id and pcr.AccountID = " + AccountId + " and pcr.ProjectID = " + ProjectId + " and pcr.Year = (select top(1) pcr.Year from ProcessComplianceResults pcr where pcr.AccountID=" + AccountId + " and pcr.ProjectID= " + ProjectId + " order by year desc, quarter desc) and pcr.Quarter = (select top(1) pcr.Quarter from ProcessComplianceResults pcr where pcr.AccountID=" + AccountId + " and pcr.ProjectID= " + ProjectId + "  order by year desc,quarter desc)");
            }
            else
            {
                builder.Append(" where pcr.QualityParameterId = pcqr.Id and pcr.AccountID = " + AccountId + " and pcr.ProjectID = " + ProjectId + " and pcr.Year = " + year + " and pcr.Quarter = " + quarter + "");
            }
            return this._db.Query<ProcessComplianceResults>(builder.ToString()).ToList();
        }

        public int add(ProcessComplianceResults results)
        {

            int datarows = 0;

            datarows = this._db.Execute(@"INSERT ProcessComplianceResults([AccountID],[ProjectID],[Year],[Quarter],[QualityParameterId],[Rating]) values (@AccountId,@ProjectId,@Year,@Quarter,@QualityParameterId,@Rating)",
                new { AccountId = results.AccountId, ProjectId = results.ProjectId, Year = results.Year, Quarter = results.Quarter, QualityParameterId = results.QualityParameterId, Rating = results.Rating});

            return datarows;
        }
    }
}

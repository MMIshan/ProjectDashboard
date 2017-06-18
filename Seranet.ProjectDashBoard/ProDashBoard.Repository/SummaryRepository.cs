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
    public class SummaryRepository : ISummaryRepository
    {

        private readonly IDbConnection _db;

        public SummaryRepository()
        {
            _db = new SqlConnection(ConfigurationManager.ConnectionStrings["DashBoard1"].ConnectionString);
        }
        public List<Summary> Get()
        {
            return this._db.Query<Summary>("SELECT [Id],[Year],[Quarter],[Rating] FROM [Summary]").ToList();
        }
        
        public List<Summary> getSelectedProjectSummaries(int projectId)
        {
            return this._db.Query<Summary>("SELECT [Id],[ProjectId],[Year],[Quarter],[Rating] FROM [Summary] where ProjectId='"+projectId+"' order by Year asc,Quarter asc").ToList();
        }

        public Summary getLatestProjectSummary(int projectId,int year,int quarter)
        {
            String query = "SELECT Top(1) Percent [Id],[ProjectId],[Year],[Quarter],[Rating] FROM [Summary] where ProjectId='" + projectId + "' order by Year desc,Quarter desc";
            if (year != 0 | quarter != 0) {
                query = "SELECT [Id],[ProjectId],[Year],[Quarter],[Rating] FROM [Summary] where ProjectId='" + projectId + "' and year='"+year+"' and quarter='"+quarter+"'";
            }
            
            return this._db.Query<Summary>(query).SingleOrDefault();
        }

        public int updateSelectedSummary(int accountId,int year,int quarter,double rating) {
            int rowsAffected = this._db.Execute("UPDATE [Summary] SET [Rating] = '"+rating+"' WHERE ProjectID ='"+accountId+"' and Year='"+year+"' and Quarter='"+quarter+"' ");
            if (rowsAffected == 0) {
                rowsAffected=this._db.Execute(@"INSERT Summary([ProjectID],[Year],[Quarter],[Rating]) values (@AccountId,@Year,@Quarter,@Rating)",
                new { AccountId = accountId, Year = year, Quarter = quarter, Rating = rating });
            }

            return rowsAffected;

        }
    }
}
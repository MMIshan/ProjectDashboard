using Dapper;
using ProDashBoard.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using ProDashBoard.Model.Repository;

namespace ProDashBoard.Data
{
    public class ProcessComplianceQPRepository : IProcessComplianceQPRepository
    {
        private readonly IDbConnection _db;


        public ProcessComplianceQPRepository()
        {
            _db = new SqlConnection(ConfigurationManager.ConnectionStrings["DashBoard1"].ConnectionString);
        }

        public List<ProcessComplianceQualityParameters> get()
        {
            return this._db.Query<ProcessComplianceQualityParameters>("select * from [ProcessComplianceQualityParameters] where Status=1 order by ParameterOrder asc").ToList();
        }
    }
}
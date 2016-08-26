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

namespace ProDashBoard.DAL.CommonDataRepo
{
    public class CommonDataRepository : ICommonDataRepository
    {
        private readonly IDbConnection _db;

        public CommonDataRepository()
        {
            _db = new SqlConnection(ConfigurationManager.ConnectionStrings["DashBoard1"].ConnectionString);
        }
        public CommonData getSelectedProjectCommonData(int projectid)
        {
            CommonData data=_db.Query<CommonData>("SELECT * FROM [CommonData] WHERE ProjectId = '" +projectid+ "'").SingleOrDefault();
            //Debug.WriteLine("PCLINK "+data.ProcessComplianceLink+" "+ data.Id+" "+data.ProjectId);
            return data;
        }
    }
}

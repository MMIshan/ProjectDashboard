using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using ProDashBoard.Models;
using Dapper;
using ProDashBoard.Model.Repository;

namespace ProDashBoard.Data
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly IDbConnection _db;

        public QuestionRepository()
        {
            _db = new SqlConnection(ConfigurationManager.ConnectionStrings["DashBoard1"].ConnectionString);
        }

        public List<Questions> Get()
        {
            return this._db.Query<Questions>("SELECT [Id],[QuestionName],[QuestionNote],[QuestionHint],[QuestionType],[Availability],[CalcExist],[QuestionOrder],[Mandatory],[Comment],[MaxValue] FROM [Questions] WHERE Availability=1 order by QuestionOrder asc").ToList();
        }
        
        public Questions Get(int id)
        {
            throw new NotImplementedException();
        }

        public List<Questions> getDisplayingQuestions(int availability)
        {
            String query = "SELECT [Id],[QuestionName],[QuestionNote],[QuestionHint],[QuestionType],[Availability],[CalcExist],[QuestionOrder],[Mandatory],[Comment],[MaxValue] FROM [Questions] WHERE CalcExist=1 order by QuestionOrder asc";
            if (availability == 1) {
                query = "SELECT [Id],[QuestionName],[QuestionNote],[QuestionHint],[QuestionType],[Availability],[CalcExist],[QuestionOrder],[Mandatory],[Comment],[MaxValue] FROM [Questions] WHERE CalcExist=1 and Availability=1 order by QuestionOrder asc";
            }
            return this._db.Query<Questions>(query).ToList();
        }
    }
}
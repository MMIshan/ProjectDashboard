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

namespace ProDashBoard.DAL
{
    public class TeamMemberRepository : ITeamMemberRepository
    {
        private readonly IDbConnection _db;

        public TeamMemberRepository()
        {
            _db = new SqlConnection(ConfigurationManager.ConnectionStrings["DashBoard1"].ConnectionString);
        }
        public List<TeamMembers> Get()
        {
            return this._db.Query<TeamMembers>("SELECT [Id],[MemberName],[AdminRights] FROM TeamMembers").ToList();
        }

        public TeamMembers Get(int id)
        {
            throw new NotImplementedException();
        }

        public TeamMembers getSelectedEmployee(string username)
        {
            
            TeamMembers mem= this._db.Query<TeamMembers>("SELECT [Id],[MemberName],[AdminRights] FROM TeamMembers WHERE MemberName='" + username + "'").SingleOrDefault();
            
            return mem; 
        }

        public List<TeamMembers> getActiveTeamMembers()
        {
            throw new NotImplementedException();
        }
    }
}
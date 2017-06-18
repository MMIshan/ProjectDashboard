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
using ProDashBoard.Model.Repository;

namespace ProDashBoard.Data
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
            return this._db.Query<TeamMembers>("SELECT * FROM TeamMembers where Availability=1 order by MemberName asc ").ToList();
        }

        public TeamMembers Get(int id)
        {
            TeamMembers mem = this._db.Query<TeamMembers>("SELECT * FROM TeamMembers WHERE Id='" + id + "'").SingleOrDefault();

            return mem;
        }

        public TeamMembers getSelectedEmployee(string username)
        {
            
            TeamMembers mem= this._db.Query<TeamMembers>("SELECT * FROM TeamMembers WHERE MemberName='" + username + "'").SingleOrDefault();
            
            return mem; 
        }

        public List<TeamMembers> getActiveTeamMembers()
        {
            throw new NotImplementedException();
        }

        public int add(TeamMembers teamMembers)
        {

            int datarows = 0;
            if (teamMembers.MemberName != null) { 
            datarows = this._db.Execute(@"INSERT TeamMembers([MemberName],[AdminRights],[Availability]) values (@MemberName,@AdminRights,@Availability)",
                new { MemberName = teamMembers.MemberName, AdminRights = teamMembers.AdminRights, Availability = teamMembers.Availability});
            }
            return datarows;
        }
    }
}

using Dapper;
using ProDashBoard.DAL.SettingsFileRepo;
using ProDashBoard.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace ProDashBoard.DAL
{
    public class ProjectRepository : IProjectRepository
    {

        private readonly IDbConnection _db;
        
        public ProjectRepository()
        {
            _db = new SqlConnection(ConfigurationManager.ConnectionStrings["DashBoard1"].ConnectionString);
            
            
        }

        public List<Project> Get()
        {
            Debug.WriteLine("Threshold");
            return this._db.Query<Project>("SELECT [Id],[AccountId],[Name],[ProjetCode],[Enabled],[RowVersion],[ProjectOwner] FROM [Project] WHERE Enabled=1").ToList();
        }

        public Project Get(int id)
        {
            return _db.Query<Project>("SELECT [Id],[AccountId],[Name],[ProjetCode],[Enabled],[RowVersion],[ProjectOwner] FROM [Project] WHERE Enabled=1 AND ID = '" + id+"'").SingleOrDefault();
        }

        public List<ProjectData> getProjectData()
        {
            return _db.Query<ProjectData>("SELECT p.[Id],p.[AccountId],a.[AccountName],p.[Name],p.[ProjetCode],p.[Enabled],p.[RowVersion],p.[ProjectOwner] FROM [Project] p,[Account] a WHERE p.AccountId=a.Id and p.Enabled=1").ToList();
        }

        public Spec GetSpec(int AccountId)
        {
            return _db.Query<Spec>("SELECT [Id],[AccountId],[linkId],[SpecLevel],[PendingCount] FROM [Spec] WHERE AccountId = '" + AccountId + "'").SingleOrDefault();
        }

        public List<Project> getSelectedAccountProjects(int accountId)
        {
            return _db.Query<Project>("SELECT [Id],[AccountId],[Name],[ProjetCode],[Enabled],[RowVersion],[ProjectOwner] FROM [Project] WHERE Enabled=1 AND AccountId = '" + accountId + "'").ToList();
        }

    }
}
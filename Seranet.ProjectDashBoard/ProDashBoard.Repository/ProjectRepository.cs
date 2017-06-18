using Dapper;

using ProDashBoard.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using ProDashBoard.Model.Repository;
using System.Text;
using ProDashBoard.Repository;

namespace ProDashBoard.Data
{
    public class ProjectRepository : IProjectRepository
    {

        private readonly IDbConnection _db;
        private AuthorizationRepository authRepo;
        
        public ProjectRepository()
        {
            _db = new SqlConnection(ConfigurationManager.ConnectionStrings["DashBoard1"].ConnectionString);
            //SpecRepository spec = new SpecRepository();
            //spec.getSpecLevel("comp1");
            authRepo = new AuthorizationRepository();

        }

        public List<Project> Get()
        {
            return this._db.Query<Project>("SELECT * FROM [Project] WHERE Enabled=1 order by Name asc").ToList();
        }

        public List<Project> getAdminalPanelActiveProjects()
        {
            string initialQuery = "";
            List<Project> projects = new List<Project>();
            TeamMembers member= authRepo.getLoggedInUser();
            if (authRepo.getAdminRights()) {
                initialQuery = "SELECT * FROM [Project] WHERE Enabled=1 order by Name asc";
                projects=this._db.Query<Project>(initialQuery).ToList();
            }
            else if (authRepo.getTeamLeadRightsForAnyAccount()) {
                initialQuery = "SELECT * FROM [Project] WHERE Enabled=1 and AccountId In(select distinct (e.AccountId) from EmployeeProjects e,Account ac where e.AccountId=ac.Id and ((e.EmpId=" + member.Id + " and e.Lead=1) or ac.AccountOwner=" + member.Id + ")) order by Name asc";
                projects = this._db.Query<Project>(initialQuery).ToList();
            }
            return projects;
        }

        public List<Project> getAdminalPanelInactiveProjects()
        {
            string initialQuery = "";
            List<Project> projects = new List<Project>();
            TeamMembers member = authRepo.getLoggedInUser();
            if (authRepo.getAdminRights())
            {
                initialQuery = "SELECT * FROM [Project] WHERE Enabled=0 order by Name asc";
                projects = this._db.Query<Project>(initialQuery).ToList();
            }
            else if (authRepo.getTeamLeadRightsForAnyAccount())
            {
                initialQuery = "SELECT * FROM [Project] WHERE Enabled=0 and AccountId In(select distinct (e.AccountId) from EmployeeProjects e,Account ac where e.AccountId=ac.Id and ((e.EmpId=" + member.Id+" and e.Lead=1) or ac.AccountOwner="+member.Id+")) order by Name asc";
                projects = this._db.Query<Project>(initialQuery).ToList();
            }
            return projects;
        }

        public List<Project> getInactiveProjects()
        {
            return this._db.Query<Project>("SELECT * FROM [Project] WHERE Enabled=0  order by Name asc").ToList();
        }

        //SELECT * FROM [Project] WHERE Enabled=1 and AccountId In(select distinct (e.AccountId) from EmployeeProjects e where e.EmpId=6 and e.Lead=1) order by Name asc

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

        public List<Project> getSelectedAccountActiveBillableProjects(int accountId)
        {
            return _db.Query<Project>("SELECT [Id],[AccountId],[Name],[ProjetCode],[Enabled],[RowVersion],[ProjectOwner] FROM [Project] WHERE Enabled=1 AND Billable=1 and AccountId = '" + accountId + "'").ToList();
        }

        public List<Project> getSelectedAdminAccountProjects(int accountId)
        {
            return _db.Query<Project>("SELECT [Id],[AccountId],[Name],[ProjetCode],[Enabled],[RowVersion],[ProjectOwner] FROM [Project] WHERE AccountId = '" + accountId + "'").ToList();
        }

        public int add(string projectName, int accountId)
        {
            int datarows = 0;

            if (accountId == 0)
            {
                datarows = this._db.Execute(@"INSERT Project([AccountId],[Name],[Enabled]) values ((select MAX(Id) from Account),@Name,@Enabled)",
                new { Name = projectName, Enabled = true });
            }
            else
            {
                datarows = this._db.Execute(@"INSERT Project([AccountId],[Name],[Enabled]) values (@AccountId,@Name,@Enabled)",
                    new { AccountId=accountId, Name = projectName, Enabled = true });
            }
            return datarows;
        }

        public int update(Project project)
        {
            int datarows = 0;
            StringBuilder stringBuilder = new StringBuilder("UPDATE Project set");
            if (project.Name != null) {
                stringBuilder.Append(" [Name]=@Name,");
            }
            if (project.ProjetCode != null)
            {
                stringBuilder.Append(" [ProjetCode]=@ProjetCode,");
            }
            if (project.ProjectOwner != null)
            {
                stringBuilder.Append(" [ProjectOwner]=@ProjectOwner,");
            }

            stringBuilder.Append(" [Enabled]=@Enabled WHERE Id=@Id");
            
            datarows = this._db.Execute(stringBuilder.ToString(),
                new { Name=project.Name, ProjetCode=project.ProjetCode, ProjectOwner=project.ProjectOwner, Enabled=project.Enabled,Id=project.Id });

            Debug.WriteLine("Projectdata " + datarows);
            return datarows;
        }

        public int updateFullProject(Project project)
        {
            int datarows = 0;

            datarows = this._db.Execute("UPDATE Project set [Name]=@Name, [ProjetCode]=@ProjetCode, [Enabled]=@Enabled, [ProjectOwner]=@ProjectOwner,[Description]=@Description,[Billable]=@Billable WHERE [Id]=@Id",
                new { Name = project.Name, ProjetCode = project.ProjetCode, Enabled = project.Enabled, ProjectOwner = project.ProjectOwner, Description = project.Description, Billable = project.Billable, Id=project.Id });

            Debug.WriteLine("data " + datarows);
            return datarows;
        }
    }
} 
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
using ProDashBoard.Model;

namespace ProDashBoard.Data
{
    public class ProcessComplianceSummaryRepository : IProcessComplianceSummaryRepository
    {
        private readonly IDbConnection _db;
        private ProjectRepository projectRepo;

        public ProcessComplianceSummaryRepository()
        {
            _db = new SqlConnection(ConfigurationManager.ConnectionStrings["DashBoard1"].ConnectionString);
            projectRepo = new ProjectRepository();
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
                data = this._db.Query("select (select top(1) pcs.Year from ProcessComplianceSummary pcs where pcs.AccountId='" + AccountId + "' order by pcs.Year desc,pcs.Quarter desc) as year,(select top(1) PERCENT pcs.Quarter from ProcessComplianceSummary pcs where pcs.AccountId='" + AccountId + "' order by pcs.Year desc,pcs.Quarter desc) as quarter,(sum(pcs.Rating)/(select count(*) from ProcessComplianceSummary pcs1 where pcs1.Year=(select top(1) PERCENT pcs.year from ProcessComplianceSummary pcs order by pcs.Year desc,pcs.Quarter desc) and pcs1.Quarter=(select top(1) PERCENT pcs.Quarter from ProcessComplianceSummary pcs order by pcs.Year desc,pcs.Quarter desc) and pcs1.Rating!=0 and pcs1.AccountId='" + AccountId + "')) as average,((select count(*) from Project p where p.Enabled='" + AccountId + "' and p.AccountId='" + AccountId + "')-(select count(pcs1.Id) from ProcessComplianceSummary pcs1 where pcs1.Year=(select top(1) PERCENT pcs.Year from ProcessComplianceSummary pcs order by pcs.Year desc,pcs.Quarter desc) and pcs1.Quarter=(select top(1) PERCENT pcs.Quarter from ProcessComplianceSummary pcs order by pcs.Year desc,pcs.Quarter desc) and pcs1.Rating!=0 and pcs1.AccountId='" + AccountId + "' )) as difference from ProcessComplianceSummary pcs where pcs.Year=(select top(1) PERCENT pcs.Year from ProcessComplianceSummary pcs where pcs.AccountId='" + AccountId + "' order by pcs.Year desc,pcs.Quarter desc) and pcs.Quarter=(select top(1) PERCENT pcs.Quarter from ProcessComplianceSummary pcs where pcs.AccountId='" + AccountId + "' order by pcs.Year desc,pcs.Quarter desc) and pcs.AccountId='" + AccountId + "'").Select(d => new object[] { d.year, d.quarter, d.average, d.difference }).SingleOrDefault();
               
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

            datarows = this._db.Execute(@"INSERT ProcessComplianceSummary([AccountID],[ProjectID],[Year],[Quarter],[Rating],[ProcessVersion]) values (@AccountId,@ProjectId,@Year,@Quarter,@Rating,@ProcessVersion)",
                new { AccountId = summary.AccountId, ProjectId = summary.ProjectId, Year = summary.Year, Quarter = summary.Quarter,Rating = summary.Rating, ProcessVersion=summary.ProcessVersion });

            return datarows;
        }

        public List<ProcessComplianceWidgetData> getWidgetData(int AccountId)
        {
            List<Object[]> returnData = this._db.Query("select cs1.Rating as Rating,cs1.Year as Year,cs1.Quarter as Quarter,cs1.ProjectId as ProjectId  from ProcessComplianceSummary cs1 where cs1.AccountId=" + AccountId + " and cs1.Year=(select top(1) cs.YEAR from ProcessComplianceSummary cs where AccountId=cs1.AccountId order by cs.Year desc ) and cs1.Quarter=(select top(1) cs.Quarter from ProcessComplianceSummary cs where AccountId=cs1.AccountId order by cs.Year desc,cs.Quarter desc)").Select(d => new object[] { d.Rating, d.Year, d.Quarter, d.ProjectId }).ToList();
            List<ProcessComplianceWidgetData> widgetData = new List<ProcessComplianceWidgetData>();
            if (returnData.Count != 0)
            {
                List<Project> activeProjects = projectRepo.getSelectedAccountProjects(AccountId);
                if (activeProjects.Count != 0)
                {
                    foreach (Project project in activeProjects)
                    {
                        ProcessComplianceWidgetData processComplianceWidgetData = new ProcessComplianceWidgetData();
                        processComplianceWidgetData.Project = project;
                        processComplianceWidgetData.Rating = -1;
                        foreach (Object[] dataArray in returnData)
                        {
                            processComplianceWidgetData.Year = Convert.ToInt32(dataArray[1]);
                            processComplianceWidgetData.Quarter = Convert.ToInt32(dataArray[2]);
                            if (project.Id == Convert.ToInt32(dataArray[3]))
                            {
                                processComplianceWidgetData.Rating = Convert.ToDouble(dataArray[0]);
                            }
                        }
                        widgetData.Add(processComplianceWidgetData);
                    }
                }

            }
            return widgetData;
        }
    }
}

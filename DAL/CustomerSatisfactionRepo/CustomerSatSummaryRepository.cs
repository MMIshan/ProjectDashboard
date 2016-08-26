using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProDashBoard.Models;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;

namespace ProDashBoard.DAL.CustomerSatisfactionRepo
{
    public class CustomerSatSummaryRepository : ICustomerSatSummaryRepository
    {
        private readonly IDbConnection _db;
        private ProjectRepository projectRepo;

        public CustomerSatSummaryRepository()
        {
            projectRepo = new ProjectRepository();
            _db = new SqlConnection(ConfigurationManager.ConnectionStrings["DashBoard1"].ConnectionString);
        }
        public List<CustomerSatSummary> Get()
        {
            return null;
        }

        public List<List<CustomerSatSummary>> getSelectedAccountSummaries(int accountId)
        {
            List<List<CustomerSatSummary>> cusSatSummaryList = new List<List<CustomerSatSummary>>();
            List<Project> projectList=projectRepo.getSelectedAccountProjects(accountId);
            if (projectList.Count != 0) {
                foreach (Project pro in projectList) {
                    List<CustomerSatSummary> tempList=this._db.Query<CustomerSatSummary>("SELECT [Id],[AccountId],[ProjectId],[Year],[Quarter],[Rating] FROM [CustomerSatisfactionSummary] where AccountId='"+accountId+"' and ProjectId='" + pro.Id + "' order by Year asc,Quarter asc").ToList();
                    cusSatSummaryList.Add(tempList);
                }
            }
            return cusSatSummaryList;
        }

        public CustomerSatSummary getSelectedProjectSummary(int AccountId, int ProjectId, int year, int quarter)
        {
            return this._db.Query<CustomerSatSummary>("SELECT [Id],[AccountId],[ProjectId],[Year],[Quarter],[Rating] FROM [CustomerSatisfactionSummary] where AccountId='" + AccountId + "' and ProjectId='" + ProjectId + "' and Year='"+year+"' and Quarter='"+quarter+"'").SingleOrDefault();
        }

        //select (select top(1) PERCENT cs.Year from CustomerSatisfactionSummary cs order by cs.Year desc,cs.Quarter desc) as Y,(select top(1) PERCENT cs.Quarter from CustomerSatisfactionSummary cs order by cs.Year desc,cs.Quarter desc) as M,(sum(cs.Rating)/count(*)) as average,((select count(*) from Project p where p.AccountId=1)-count(*)) as Difference from CustomerSatisfactionSummary cs where cs.Year=(select top(1) PERCENT cs.Year from CustomerSatisfactionSummary cs order by cs.Year desc,cs.Quarter desc) and cs.Quarter=(select top(1) PERCENT cs.Quarter from CustomerSatisfactionSummary cs order by cs.Year desc,cs.Quarter desc)
        //String query = "select t.MemberName,r.Year,r.Quarter,p.AccountName,q.QuestionName,r.Answer,ISNULL(r.Comment,'-') as Comment from Results r,Questions q,TeamMembers t,Account p where r.QuestionID=q.id and r.MemberId=t.id and r.AccountID=p.id and r.AccountID='" + projectId+"' and r.Year='"+year+"' and r.Quarter='"+quarter+"' and r.MemberId='"+employeeId+"'";
        //    reviewData = _db.Query(query).Select(d => new object[] { d.MemberName, d.Year, d.Quarter,d.AccountName, d.QuestionName, d.Answer,d.Comment
        //}).ToList();
        //select (select top(1) PERCENT cs.Year from CustomerSatisfactionSummary cs where cs.AccountId=1 order by cs.Year desc,cs.Quarter desc) as Y,(select top(1) PERCENT cs.Quarter from CustomerSatisfactionSummary cs where cs.AccountId=1 order by cs.Year desc,cs.Quarter desc) as M,(sum(cs.Rating)/(select count(*) from CustomerSatisfactionSummary cs1 where cs1.Year=(select top(1) PERCENT cs.year from CustomerSatisfactionSummary cs order by cs.Year desc,cs.Quarter desc) and cs1.Quarter=(select top(1) PERCENT cs.Quarter from CustomerSatisfactionSummary cs order by cs.Year desc,cs.Quarter desc) and cs1.Rating!=0 and cs1.AccountId=1)) as average,((select count(*) from Project p where p.Enabled=1 and p.AccountId=1)-(select count(cs1.Id) from CustomerSatisfactionSummary cs1 where cs1.Year=(select top(1) PERCENT cs.Year from CustomerSatisfactionSummary cs order by cs.Year desc,cs.Quarter desc) and cs1.Quarter=(select top(1) PERCENT cs.Quarter from CustomerSatisfactionSummary cs order by cs.Year desc,cs.Quarter desc) and cs1.Rating!=0 and cs1.AccountId=1 )) as Difference from CustomerSatisfactionSummary cs where cs.Year=(select top(1) PERCENT cs.Year from CustomerSatisfactionSummary cs where cs.AccountId=1 order by cs.Year desc,cs.Quarter desc) and cs.Quarter=(select top(1) PERCENT cs.Quarter from CustomerSatisfactionSummary cs where cs.AccountId=1 order by cs.Year desc,cs.Quarter desc) and cs.AccountId=1
        public Object[] getCustomerSatisfactionWidgetDetails(int AccountId)
        {
           // return this._db.Query<Object[]>("SELECT [Id],[AccountId],[ProjectId],[Year],[Quarter],[Rating] FROM [CustomerSatisfactionSummary] where AccountId='" + AccountId + "' and ProjectId='" + ProjectId + "' and Year='" + year + "' and Quarter='" + quarter + "'").SingleOrDefault();
            return this._db.Query("select (select top(1) PERCENT cs.Year from CustomerSatisfactionSummary cs where cs.AccountId='"+ AccountId + "' order by cs.Year desc,cs.Quarter desc) as year,(select top(1) PERCENT cs.Quarter from CustomerSatisfactionSummary cs where cs.AccountId='"+ AccountId + "' order by cs.Year desc,cs.Quarter desc) as quarter,(sum(cs.Rating)/(select count(*) from CustomerSatisfactionSummary cs1 where cs1.Year=(select top(1) PERCENT cs.year from CustomerSatisfactionSummary cs order by cs.Year desc,cs.Quarter desc) and cs1.Quarter=(select top(1) PERCENT cs.Quarter from CustomerSatisfactionSummary cs order by cs.Year desc,cs.Quarter desc) and cs1.Rating!=0 and cs1.AccountId='"+ AccountId + "')) as average,((select count(*) from Project p where p.Enabled=1 and p.AccountId='"+ AccountId + "')-(select count(cs1.Id) from CustomerSatisfactionSummary cs1 where cs1.Year=(select top(1) PERCENT cs.Year from CustomerSatisfactionSummary cs order by cs.Year desc,cs.Quarter desc) and cs1.Quarter=(select top(1) PERCENT cs.Quarter from CustomerSatisfactionSummary cs order by cs.Year desc,cs.Quarter desc) and cs1.Rating!=0 and cs1.AccountId='"+ AccountId + "' )) as difference from CustomerSatisfactionSummary cs where cs.Year=(select top(1) PERCENT cs.Year from CustomerSatisfactionSummary cs where cs.AccountId='"+ AccountId + "' order by cs.Year desc,cs.Quarter desc) and cs.Quarter=(select top(1) PERCENT cs.Quarter from CustomerSatisfactionSummary cs where cs.AccountId='"+ AccountId + "' order by cs.Year desc,cs.Quarter desc) and cs.AccountId='"+ AccountId + "'").Select(d => new object[] { d.year, d.quarter, d.average, d.difference}).SingleOrDefault();
        }
    }
}
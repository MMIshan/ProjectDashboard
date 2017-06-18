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
    public class ResultsRepository : IResultsRepository
    {

        private readonly IDbConnection _db;
        private AuthorizationRepository authRepo;

        public ResultsRepository()
        {
            _db = new SqlConnection(ConfigurationManager.ConnectionStrings["DashBoard1"].ConnectionString);
            authRepo = new AuthorizationRepository();
        }

        public List<SatisfactionResults> Get()
        {
            return this._db.Query<SatisfactionResults>("SELECT [Id],[MemberId],[AccountID],[ProjectID],[Year],[Quarter],[QuestionID],[Answer],[Comment] FROM [Results]").ToList();
        }

        public SatisfactionResults Get(int id)
        {
            return this._db.Query<SatisfactionResults>("SELECT [Id],[MemberId],[AccountID],[ProjectID],[Year],[Quarter],[QuestionID],[Answer],[Comment] FROM [Results]").SingleOrDefault();
        }

        public List<List<object[]>> getSelectedResults(int projectid,int year,int quarter)
        {
            List<List<Object[]>> sendingArray = new List<List<object[]>>();
            QuestionRepository questionRepository = new QuestionRepository();
            List<Questions> displayingQuestion = questionRepository.getDisplayingQuestions(0);
            if (displayingQuestion.Count != 0)
            {
                
                foreach (Questions q1 in displayingQuestion)
                {
                    double total = 0;
                    Object[] myarray = new Object[6];
                    string query = "";
                    if (authRepo.getAdminRights() | authRepo.getTeamLeadRights(projectid))
                    {
                        query = @"select m.id,m.MemberName,q.QuestionName,r.Answer,r.Year,r.Quarter from results r, Questions q,TeamMembers m where r.QuestionID = q.Id and r.MemberId = m.Id and r.AccountID = '" + projectid + "' and q.CalcExist = 1 and r.QuestionID = '" + q1.Id + "' and r.Year = (select top(1) r.Year from results r, Questions q where r.QuestionID = q.Id and r.AccountID = '" + projectid + "' and q.CalcExist = 1 order by year desc, quarter desc) and r.Quarter = (select top(1) r.quarter from results r, Questions q where r.QuestionID = q.Id and r.AccountID = '" + projectid + "' and q.CalcExist = 1 order by year desc, quarter desc) order by m.id asc";
                        Debug.WriteLine("Year " + year + " " + quarter + " " + (year != 0));
                        if (year != 0 | quarter != 0)
                        {
                            query = @"select m.id,m.MemberName,q.QuestionName,r.Answer,r.Year,r.Quarter from results r, Questions q,TeamMembers m where r.QuestionID = q.Id and r.MemberId = m.Id and r.AccountID = '" + projectid + "' and q.CalcExist = 1 and r.QuestionID = '" + q1.Id + "' and r.Year = '" + year + "' and r.Quarter = '" + quarter + "' order by m.id asc";
                        }
                    }
                    else
                    {

                        query = @"select m.id,m.MemberName,q.QuestionName,r.Answer,r.Year,r.Quarter from results r, Questions q,TeamMembers m where r.QuestionID = q.Id and r.MemberId = m.Id and r.AccountID = '" + projectid + "' and m.MemberName='"+authRepo.getUsername()+"' and q.CalcExist = 1 and r.QuestionID = '" + q1.Id + "' and r.Year = (select top(1) r.Year from results r, Questions q where r.QuestionID = q.Id and r.AccountID = '" + projectid + "' and q.CalcExist = 1 order by year desc, quarter desc) and r.Quarter = (select top(1) r.quarter from results r, Questions q where r.QuestionID = q.Id and r.AccountID = '" + projectid + "' and q.CalcExist = 1 order by year desc, quarter desc) order by m.id asc";
                        Debug.WriteLine("Year " + year + " " + quarter + " " + (year != 0));
                        if (year != 0 | quarter != 0)
                        {
                            query = @"select m.id,m.MemberName,q.QuestionName,r.Answer,r.Year,r.Quarter from results r, Questions q,TeamMembers m where r.QuestionID = q.Id and r.MemberId = m.Id and r.AccountID = '" + projectid + "' and m.MemberName='" + authRepo.getUsername() + "' and q.CalcExist = 1 and r.QuestionID = '" + q1.Id + "' and r.Year = '" + year + "' and r.Quarter = '" + quarter + "' order by m.id asc";
                        }
                    }
                    
                    List<Object[]> dataArray = _db.Query(query).Select(d => new object[] { d.id,d.MemberName,d.QuestionName, d.Answer,d.Year,d.Quarter }).ToList();
                    if (dataArray.Count != 0)
                    {


                        foreach (Object[] x in dataArray) {
                            total = total +  Convert.ToInt64(x[3]);
                        }


                        if (authRepo.getAdminRights() | authRepo.getTeamLeadRights(projectid))
                        {
                            myarray[3] = Math.Round(((double)(total / dataArray.Count)), 2);
                            myarray[1] = "Average";


                            dataArray.Add(myarray);
                        }
                        Debug.WriteLine("Data "+dataArray[0][0]);
                        sendingArray.Add(dataArray);
                    }
                }
                //List<Object[]> dynamicDataArray = new List<object[]>();
                //double dynamicTotal = 0;
                //foreach (List<Object[]> myList in sendingArray) {
                //    Object[] dynamicMyarray = new Object[4];
                //    int x = 0;
                //    foreach (Object[] arr in myList) {
                //        double value = Convert.ToInt64(arr[3]);
                //        try
                //        {
                //            if (dynamicDataArray[x] != null)
                //            {
                //                dynamicDataArray[x][3] = Convert.ToInt64(dynamicDataArray[x][3]) + value;
                //            }

                //        }
                //        catch (Exception e)
                //        {
                //            dynamicMyarray[3] = value;
                //            dynamicDataArray.Add(dynamicMyarray);
                //        }
                //        x++;
                //    }
                    
                //}
                //sendingArray.Add(dynamicDataArray);
            }
            return sendingArray;
        
        }

        public List<Object[]> getReviweData(int projectId,int year,int quarter,int employeeId)
        {
            List<Object[]> reviewData = new List<object[]>();
            String query = "select t.MemberName,r.Year,r.Quarter,p.AccountName,q.QuestionName,r.Answer,ISNULL(r.Comment,'-') as Comment,q.QuestionType from Results r,Questions q,TeamMembers t,Account p where r.QuestionID=q.id and r.MemberId=t.id and r.AccountID=p.id and r.AccountID='" + projectId+"' and r.Year='"+year+"' and r.Quarter='"+quarter+"' and r.MemberId='"+employeeId+"'";
            reviewData = _db.Query(query).Select(d => new object[] { d.MemberName, d.Year, d.Quarter,d.AccountName, d.QuestionName, d.Answer,d.Comment,d.QuestionType }).ToList();
            return reviewData;
        }

        public int add(SatisfactionResults results)
        {

            int datarows = 0;
            
                datarows = this._db.Execute(@"INSERT Results([MemberId],[AccountID],[ProjectID],[Year],[Quarter],[QuestionID],[Answer],[Comment]) values (@MemberId,@AccountId,@ProjectId,@Year,@Quarter,@QuestionId,@Answer,@Comment)",
                    new { MemberId = results.MemberId, AccountId = results.AccountId, ProjectId = results.ProjectId, Year = results.Year, Quarter = results.Quarter, QuestionId = results.QuestionId, Answer = results.Answer, Comment = results.Comment });
            
            return datarows;
            }


    }
}

using Dapper;
using ProDashBoard.Data;
using ProDashBoard.Model;
using ProDashBoard.Model.Repository;
using ProDashBoard.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;

namespace ProDashBoard.Repository
{
    public class FinancialDataRepository : IFinancialDataRepository
    {
        private readonly IDbConnection _db;
        private ProjectRepository projectRepo;
        private TeamMemberRepository teamRepo;
        private EmployeeProjectsRepository empRepo;
        private IAccountRepository accountRepo;
        public FinancialDataRepository()
        {
            _db = new SqlConnection(ConfigurationManager.ConnectionStrings["Financial"].ConnectionString);
            projectRepo = new ProjectRepository();
            teamRepo = new TeamMemberRepository();
            empRepo = new EmployeeProjectsRepository();
            accountRepo = new AccountRepository();
        }

        public FinancialFinalData getDataFromTimeReports(int year, int month, int accountId)
        {
            List<Project> includedActiveProjects = projectRepo.getSelectedAccountActiveBillableProjects(accountId);
            List<FinancialData> returnData = new List<FinancialData>();
            FinancialFinalData finalReturnData = new FinancialFinalData();
            finalReturnData.sendingData = returnData;
            finalReturnData.financialDataMissingMembers = "";
            List<EmployeeProjectsData> activeTeamMemberData = empRepo.getEmployeesOfSelectedAccount(accountId);
            Account selectedAccount = accountRepo.Get(accountId);
            List<string> timePortProjectCodes = new List<string>();
            if (selectedAccount != null)
            {
                try
                {
                    timePortProjectCodes = selectedAccount.AllProjectCodes.Split(',').ToList();
                }
                catch (Exception e)
                {
                    timePortProjectCodes = new List<string>();
                }
            }
            string projectListQuery = null;
            if (year != 0 & month != 0 & accountId != 0)
            {
                if (timePortProjectCodes.Count != 0)
                {
                    projectListQuery = "1!=1";

                    foreach (string code in timePortProjectCodes)
                    {
                        string projectCode = code;
                        if (projectCode != null)
                        {
                            projectListQuery = projectListQuery + " OR jte.project_key = '" + projectCode + "'";
                        }
                    }
                    StringBuilder builder = new StringBuilder();

                    string startDate = year + "-" + month + "-01";

                    builder.Append("select sum(jte.duration) as time_duration, ju.system_id,");

                    builder.Append(" (select sum(jte1.duration) from JiraTimeSheetEntries jte1, JiraUsers ju1 where jte1.user_id = ju1.user_id and ju1.system_id = ju.system_id and jte1.time_log_date between '" + startDate + "' and (SELECT DATEADD(s, -1, DATEADD(mm, DATEDIFF(m, 0, '" + startDate + "') + 1, 0)))) as total_duration");

                    builder.Append(" from[JiraTimeSheetEntries] jte,[JiraUsers] ju where jte.user_id=ju.user_id ");

                    if (projectListQuery != null)
                    {
                        builder.Append(" and (" + projectListQuery + ")");
                    }

                    //builder.Append(" and jte.time_log_date between '"+startDate+"' and (SELECT DATEADD(s,-1, DATEADD(mm, DATEDIFF(m,0,'"+startDate+ "')+1,0))) and jp.category_id='1' group by ju.system_id order by ju.system_id");

                    builder.Append(" and jte.time_log_date between '" + startDate + "' and (SELECT DATEADD(s,-1, DATEADD(mm, DATEDIFF(m,0,'" + startDate + "')+1,0))) and 1=1 group by ju.system_id order by ju.system_id");

                    Debug.WriteLine("Query   " + builder.ToString());


                    List<Object[]> resultsArray = this._db.Query(builder.ToString()).Select(d => new object[] { d.time_duration, d.system_id, d.total_duration }).ToList();

                    Debug.WriteLine("Length " + resultsArray.Count);
                    //List<EmployeeProjectsData> activeAccountMembers=empRepo.getEmployeesOfSelectedAccount(accountId);
                    foreach (Object[] dataArray in resultsArray)
                    {

                        TeamMembers member = teamRepo.getSelectedEmployee((String)dataArray[1]);
                        if (member != null)
                        {
                            List<EmployeeProjects> empProjectData = empRepo.getEmployeeProjectsForSelectedAccount(member.Id, accountId);
                            if (empProjectData.Count != 0)
                            {
                                for (int i = 0; i < activeTeamMemberData.Count; i++)
                                {
                                    if (activeTeamMemberData[i].EmpId == member.Id)
                                    {
                                        activeTeamMemberData.RemoveAt(i);
                                    }
                                }
                                FinancialData financialdata = new FinancialData();
                                double allocatedHourCount = 0;
                                int billableType = 0;
                                foreach (EmployeeProjects empProject in empProjectData)
                                {
                                    allocatedHourCount = allocatedHourCount + empProject.BillableHours;
                                    billableType = empProject.Billable;
                                }
                                financialdata.EmpId = member.Id;
                                financialdata.EmpName = member.MemberName;
                                financialdata.AccountId = accountId;
                                financialdata.AllocatedHours = allocatedHourCount;
                                financialdata.Year = year;
                                financialdata.Month = month;
                                financialdata.BillableHours = Convert.ToDouble(dataArray[0]);
                                financialdata.TotalHours = Convert.ToDouble(dataArray[2]);
                                financialdata.BillingType = billableType;
                                returnData.Add(financialdata);
                            }
                        }
                        Debug.WriteLine("resultedData " + dataArray[0] + " " + dataArray[1] + " " + dataArray[2] + " ");
                    }
                    string missingMembers = "";
                    if (activeTeamMemberData.Count != 0)
                    {
                        foreach (EmployeeProjectsData empData in activeTeamMemberData)
                        {
                            missingMembers += empData.EmpName + " , ";
                        }
                        missingMembers = missingMembers.Substring(0, missingMembers.Length - 2);
                        finalReturnData.financialDataMissingMembers = missingMembers;
                    }

                }
                //           select sum(jte.duration) as time_duration, ju.system_id, (select sum(jte1.duration) from JiraTimeSheetEntries jte1, JiraUsers ju1 where jte1.user_id = ju1.user_id and ju1.system_id = ju.system_id and jte1.time_log_date between '2014-12-01' and(SELECT DATEADD(s, -1, DATEADD(mm, DATEDIFF(m, 0, '2014-12-1') + 1, 0)))) from[JiraTimeSheetEntries] jte,[JiraUsers]
                //       ju where jte.user_id=ju.user_id and(jte.project_key= 'CQ' or jte.project_key= 'CD')
                //and jte.time_log_date between '2014-12-01' and (SELECT DATEADD(s,-1, DATEADD(mm, DATEDIFF(m,0,'2014-12-1')+1,0))) and jte.category_id='1' group by ju.system_id
                return finalReturnData;
            }
            else
            {
                return null;
            }
        }
    }
}




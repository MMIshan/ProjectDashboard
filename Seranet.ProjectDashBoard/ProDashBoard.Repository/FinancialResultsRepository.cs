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
using System.Globalization;
using System.Linq;
using System.Web;

namespace ProDashBoard.Repository
{
    public class FinancialResultsRepository : IFinancialResultsRepository
    {
        private readonly IDbConnection _db;
        private EmployeeProjectsRepository empRepo;
        private TeamMemberRepository teamRepo;
        private AuthorizationRepository authRepo;
        public FinancialResultsRepository() {
            _db = new SqlConnection(ConfigurationManager.ConnectionStrings["DashBoard1"].ConnectionString);
            empRepo = new EmployeeProjectsRepository();
            teamRepo = new TeamMemberRepository();
            authRepo = new AuthorizationRepository();
        }

        public int add(List<FinancialExpandedResults> results)
        {
            int datarows = 0;
            //String strDate = "5/" + results[0].Month + "/" + results[0].Year+ "";
            //DateTime dateTime = DateTime.ParseExact(strDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            //dateTime = dateTime.AddMonths(-1);
            List<int> empDataInAccount = new List<int>();
            List<int> usingEmpDataInAccount = new List<int>();
            if (results.Count != 0) {
                empDataInAccount = empRepo.getDistinctEmpIdsForAccounts(results[0].AccountId,results[0].Year,results[0].Quarter);
                usingEmpDataInAccount = empRepo.getDistinctEmpIdsForAccounts(results[0].AccountId, results[0].Year, results[0].Quarter);
            }
            
            foreach (FinancialExpandedResults result in results) {
                
                if (empDataInAccount.Count != 0)
                {
                    foreach (int data in empDataInAccount)
                    {
                        if (data==result.EmpId) {
                            usingEmpDataInAccount.Remove(data);
                        }
                    }
                }
               //Check the above method




                // This Add Statement Is Not Completed.....
                int tempMonth = result.Month;
                int pastMaxMonth=getEmployeeTimeReportMaxMonth(result.AccountId,result.Year,result.Quarter,result.EmpId);
                if (pastMaxMonth != 0)
                {
                    int currentMonth = result.Month;

                    int monthDifference = currentMonth - pastMaxMonth;

                    if (monthDifference > 1)
                    {
                        monthDifference = monthDifference - 1;
                        for (int i = (pastMaxMonth + 1); i <= (monthDifference + pastMaxMonth); i++)
                        {
                            result.Month = i;
                            Console.WriteLine(result.EmpName);
                            saveData(result);
                        }
                    }

                }
                else {

                    int pastMonth = 0;
                    int currentMonth = result.Month;
                    if (result.Quarter == 1)
                    {
                        pastMonth = 1;
                    }
                    else
                    {
                        pastMonth = 7;
                    }

                    int minMonthForQuarter = getAccountTimeReportMinMonthForQuarter(result.AccountId, result.Year, result.Quarter);
                    if (minMonthForQuarter == 0)
                    {
                        pastMonth = currentMonth;
                    }
                    else {
                        pastMonth = minMonthForQuarter;
                    }

                    for (int i = pastMonth; i < currentMonth; i++)
                    {
                        FinancialExpandedResults tempResult = result;
                        tempResult.Month = i;
                        Console.WriteLine(tempResult.EmpName);
                        saveData(tempResult);
                    }
                }

                
                

                double considerableHours = 0;

                double extraOrLag = result.BillableHours-result.AllocatedHours;

                if (result.BillableHours >= result.AllocatedHours)
                {
                    considerableHours = result.AllocatedHours;
                }
                else {
                    considerableHours = result.BillableHours;
                }

                Debug.WriteLine("BillableType "+ result.BillableType);
                Console.WriteLine(result.EmpName);
                result.Month = tempMonth;
                datarows = this._db.Execute(@"INSERT FinancialResults([EmpId],[EmpName],[AccountId],[AccountName],[Year],[Month],[Quarter],[BillableType],[AllocatedHours],[BillableHours],[TotalReportedHours],[ConsiderableHours],[ExtraOrLag],[CumAllocatedHours],[CumBillableHours],[CumTotalReportedHours],[CumConsiderableHours]) values (@EmpId,@EmpName,@AccountId,@AccountName,@Year,@Month,@Quarter,@BillableType,@AllocatedHours,@BillableHours,@TotalReportedHours,@ConsiderableHours,@ExtraOrLag,@CumAllocatedHours,@CumBillableHours,@CumTotalReportedHours,@CumConsiderableHours)",
               new { EmpId = result.EmpId, EmpName =result.EmpName, AccountId =result.AccountId, AccountName =result.AccountName, Year =result.Year, Month = result.Month, Quarter =result.Quarter, BillableType =result.BillableType, AllocatedHours =result.AllocatedHours, BillableHours =result.BillableHours, TotalReportedHours =result.TotalReportedHours, ConsiderableHours =considerableHours, ExtraOrLag =extraOrLag, CumAllocatedHours =result.CumAllocatedHours, CumBillableHours =result.CumBillableHours, CumTotalReportedHours =result.CumTotalReportedHours , CumConsiderableHours =result.CumConsiderableHours });
            }

            if (usingEmpDataInAccount.Count != 0 && results.Count != 0)
            {
                foreach (int data in usingEmpDataInAccount)
                {
                    FinancialExpandedResults tempFinancialResult = new FinancialExpandedResults();
                    tempFinancialResult.EmpId = data;
                    TeamMembers employee = teamRepo.Get(data);
                    tempFinancialResult.EmpName = employee.MemberName;
                    tempFinancialResult.Year = results[0].Year;
                    tempFinancialResult.Month = results[0].Month;
                    tempFinancialResult.Quarter = results[0].Quarter;
                    tempFinancialResult.AccountId = results[0].AccountId;
                    tempFinancialResult.AccountName = results[0].AccountName;
                    saveData(tempFinancialResult);
                }
            }

            return datarows;
        }





        public int changedAdd(List<FinancialResults> results)
        {
            int datarows = 0;
            //String strDate = "5/" + results[0].Month + "/" + results[0].Year+ "";
            //DateTime dateTime = DateTime.ParseExact(strDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            //dateTime = dateTime.AddMonths(-1);
            List<int> empDataInAccount = new List<int>();
            List<int> usingEmpDataInAccount = new List<int>();
            if (results.Count != 0)
            {
                empDataInAccount = empRepo.getDistinctEmpIdsForAccounts(results[0].AccountId, results[0].Year, results[0].Quarter);
                usingEmpDataInAccount = empRepo.getDistinctEmpIdsForAccounts(results[0].AccountId, results[0].Year, results[0].Quarter);
            }

            foreach (FinancialResults result in results)
            {

                if (empDataInAccount.Count != 0)
                {
                    foreach (int data in empDataInAccount)
                    {
                        if (data == result.EmpId)
                        {
                            usingEmpDataInAccount.Remove(data);
                        }
                    }
                }
                //Check the above method




                // This Add Statement Is Not Completed.....
                int tempMonth = result.Month;
                int pastMaxMonth = getEmployeeTimeReportMaxMonth(result.AccountId, result.Year, result.Quarter, result.EmpId);
                if (pastMaxMonth != 0)
                {
                    int currentMonth = result.Month;

                    int monthDifference = currentMonth - pastMaxMonth;

                    if (monthDifference > 1)
                    {
                        monthDifference = monthDifference - 1;
                        for (int i = (pastMaxMonth + 1); i <= (monthDifference + pastMaxMonth); i++)
                        {
                            result.Month = i;
                            Console.WriteLine(result.EmpName);
        //                    saveData(result);
                        }
                    }

                }
                else
                {

                    int pastMonth = 0;
                    int currentMonth = result.Month;
                    if (result.Quarter == 1)
                    {
                        pastMonth = 1;
                    }
                    else
                    {
                        pastMonth = 7;
                    }

                    int minMonthForQuarter = getAccountTimeReportMinMonthForQuarter(result.AccountId, result.Year, result.Quarter);
                    if (minMonthForQuarter == 0)
                    {
                        pastMonth = currentMonth;
                    }
                    else
                    {
                        pastMonth = minMonthForQuarter;
                    }

                    for (int i = pastMonth; i < currentMonth; i++)
                    {
                        FinancialResults tempResult = result;
                        tempResult.Month = i;
                        Console.WriteLine(tempResult.EmpName);
         //               saveData(tempResult);
                    }
                }




                double considerableHours = 0;

                double extraOrLag = result.BillableHours - result.AllocatedHours;

                if (result.BillableHours >= result.AllocatedHours)
                {
                    considerableHours = result.AllocatedHours;
                }
                else
                {
                    considerableHours = result.BillableHours;
                }

                Debug.WriteLine("BillableType " + result.BillableType);
                Console.WriteLine(result.EmpName);
                result.Month = tempMonth;
                datarows = this._db.Execute(@"INSERT FinancialResults([EmpId],[EmpName],[AccountId],[AccountName],[Year],[Month],[Quarter],[BillableType],[AllocatedHours],[BillableHours],[TotalReportedHours],[ConsiderableHours],[ExtraOrLag]) values (@EmpId,@EmpName,@AccountId,@AccountName,@Year,@Month,@Quarter,@BillableType,@AllocatedHours,@BillableHours,@TotalReportedHours,@ConsiderableHours,@ExtraOrLag)",
               new { EmpId = result.EmpId, EmpName = result.EmpName, AccountId = result.AccountId, AccountName = result.AccountName, Year = result.Year, Month = result.Month, Quarter = result.Quarter, BillableType = result.BillableType, AllocatedHours = result.AllocatedHours, BillableHours = result.BillableHours, TotalReportedHours = result.TotalReportedHours, ConsiderableHours = considerableHours, ExtraOrLag = extraOrLag });
            }

            if (usingEmpDataInAccount.Count != 0 && results.Count != 0)
            {
                foreach (int data in usingEmpDataInAccount)
                {
                    FinancialResults tempFinancialResult = new FinancialResults();
                    tempFinancialResult.EmpId = data;
                    TeamMembers employee = teamRepo.Get(data);
                    tempFinancialResult.EmpName = employee.MemberName;
                    tempFinancialResult.Year = results[0].Year;
                    tempFinancialResult.Month = results[0].Month;
                    tempFinancialResult.Quarter = results[0].Quarter;
                    tempFinancialResult.AccountId = results[0].AccountId;
                    tempFinancialResult.AccountName = results[0].AccountName;
       //             saveData(tempFinancialResult);
                }
            }

            return datarows;
        }

        public void saveData(FinancialExpandedResults result) {
            this._db.Execute(@"INSERT FinancialResults([EmpId],[EmpName],[AccountId],[AccountName],[Year],[Month],[Quarter],[BillableType],[AllocatedHours],[BillableHours],[TotalReportedHours],[ConsiderableHours],[ExtraOrLag],[CumAllocatedHours],[CumBillableHours],[CumTotalReportedHours],[CumConsiderableHours]) values (@EmpId,@EmpName,@AccountId,@AccountName,@Year,@Month,@Quarter,@BillableType,@AllocatedHours,@BillableHours,@TotalReportedHours,@ConsiderableHours,@ExtraOrLag,@CumAllocatedHours,@CumBillableHours,@CumTotalReportedHours,@CumConsiderableHours)",
                            new { EmpId = result.EmpId, EmpName = result.EmpName, AccountId = result.AccountId, AccountName = result.AccountName, Year = result.Year, Month = result.Month, Quarter = result.Quarter, BillableType = 4, AllocatedHours = -1, BillableHours = -1, TotalReportedHours = -1, ConsiderableHours = -1, ExtraOrLag = -1, CumAllocatedHours=-1, CumBillableHours=-1, CumTotalReportedHours=-1, CumConsiderableHours=-1 });
        }


        public List<List<FinancialExpandedResults>> getSelectedFinancialResults(int year, int quarter, int accountId)
        {
            List<List<FinancialExpandedResults>> returnList = new List<List<FinancialExpandedResults>>();
            List<int> reportedEmpIds = getTimeReportedEmpIdForQuarter(year, quarter, accountId);

            if (reportedEmpIds.Count != 0)
            {
                if (authRepo.getAdminRights() || authRepo.getTeamLeadRights(accountId)) {
                    foreach (int empId in reportedEmpIds)
                    {
                        string query = "SELECT [Id],[EmpId],[EmpName],[AccountId],[AccountName],[Year],[Month],[Quarter],[BillableType],[AllocatedHours],[BillableHours],[TotalReportedHours],[ConsiderableHours],[ExtraOrLag] FROM [FinancialResults] WHERE Year='" + year + "' and Quarter='" + quarter + "' and AccountId='" + accountId + "' and EmpId='" + empId + "' order by EmpId asc,Month asc";
                        if (year == 0 & quarter == 0)
                        {
                            query = "SELECT [Id],[EmpId],[EmpName],[AccountId],[AccountName],[Year],[Month],[Quarter],[BillableType],[AllocatedHours],[BillableHours],[TotalReportedHours],[ConsiderableHours],[ExtraOrLag] FROM [FinancialResults] WHERE Year=(select max(year) from FinancialResults where [AccountId ]=" + accountId + ") and Quarter=(select top(1) [Quarter ] from FinancialResults where [AccountId ]=" + accountId + " order by YEAR desc, [Quarter ] desc) and AccountId=" + accountId + " and EmpId='" + empId + "'  order by EmpId asc,Month asc";
                        }
                        Debug.WriteLine("Results " + query);
                        returnList.Add(this._db.Query<FinancialExpandedResults>(query).ToList());
                    }
}
                else {
                    TeamMembers member = authRepo.getLoggedInUser();
                    string query = "SELECT [Id],[EmpId],[EmpName],[AccountId],[AccountName],[Year],[Month],[Quarter],[BillableType],[AllocatedHours],[BillableHours],[TotalReportedHours],[ConsiderableHours],[ExtraOrLag] FROM [FinancialResults] WHERE Year='" + year + "' and Quarter='" + quarter + "' and AccountId='" + accountId + "' and EmpId='" + member.Id + "' order by EmpId asc,Month asc";
                    if (year == 0 & quarter == 0)
                    {
                        query = "SELECT [Id],[EmpId],[EmpName],[AccountId],[AccountName],[Year],[Month],[Quarter],[BillableType],[AllocatedHours],[BillableHours],[TotalReportedHours],[ConsiderableHours],[ExtraOrLag] FROM [FinancialResults] WHERE Year=(select max(year) from FinancialResults where [AccountId ]=" + accountId + ") and Quarter=(select top(1) [Quarter ] from FinancialResults where [AccountId ]=" + accountId + " order by YEAR desc, [Quarter ] desc) and AccountId=" + accountId + " and EmpId='" + member.Id + "'  order by EmpId asc,Month asc";
                    }
                    Debug.WriteLine("Results " + query);
                    returnList.Add(this._db.Query<FinancialExpandedResults>(query).ToList());
                }

            }

            return returnList;
        }

        public List<int> getTimeReportedEmpIdForQuarter(int year, int quarter, int accountId) {

            string query = @"SELECT distinct [EmpId ] as EmpId FROM [FinancialResults] WHERE Year="+year+" and Quarter="+quarter+" and AccountId="+accountId+" ";
            if(year==0 & quarter == 0)
            {
                query = @"SELECT distinct [EmpId ] as EmpId FROM [FinancialResults] WHERE Year=(select max(year) from FinancialResults where [AccountId ]=" + accountId + ") and Quarter=(select top(1) [Quarter ] from FinancialResults where [AccountId ]=" + accountId + " order by YEAR desc, [Quarter ] desc) and AccountId=" + accountId + " ";
            }
            return this._db.Query<int>(query).ToList();
        }

        //check this method
        public int getEmployeeTimeReportMaxMonth(int accountId,int year,int quarter,int empId) {
            string query = "select max(month) as month from FinancialResults where AccountId='" + accountId+"' and Year='"+year+"' and Quarter='"+quarter+ "' and EmpId='"+empId+"' order by month asc";

            string returnedData=this._db.Query<string>(query).SingleOrDefault();

            if (returnedData != null)
            {
                return Convert.ToInt32(returnedData);
            }
            else {
                return 0;
            }
        }

        //check this method as wel.....can merge with above method for one query
        public int getEmployeeTimeReportMaxYear(int accountId, int empId)
        {
            string query = "select max(year) as month from FinancialResults where AccountId='" + accountId + "' and EmpId='" + empId + "' order by month asc";

            string returnedData = this._db.Query<string>(query).SingleOrDefault();

            if (returnedData != null)
            {
                return Convert.ToInt32(returnedData);
            }
            else
            {
                return 0;
            }
        }

        public int getAccountTimeReportMinMonthForQuarter(int accountId, int year, int quarter)
        {
            string query = "select min(month) as month from FinancialResults where AccountId='" + accountId + "' and Year='" + year + "' and Quarter='" + quarter + "' order by month asc";

            string returnedData = this._db.Query<string>(query).SingleOrDefault();

            if (returnedData != null)
            {
                return Convert.ToInt32(returnedData);
            }
            else
            {
                return 0;
            }
        }

        public int deleteFinancialResults(int accountId,int year,int quarter,int month)
        {
           return this._db.Execute(@"delete from FinancialResults where AccountId="+accountId+" and Year="+year+" and Quarter="+quarter+" and Month="+month+" ");
        }

        public FinancialExpandedResults getIndividualEmpDataForLastMonthInQuarter(int accountId,int empId,int year,int quarter) {

            string query = "SELECT top(1) [Id],[EmpId],[EmpName],[AccountId],[AccountName],[Year],[Month],[Quarter],[BillableType],[AllocatedHours],[BillableHours],[TotalReportedHours],[ConsiderableHours],[ExtraOrLag],[CumAllocatedHours],[CumBillableHours],[CumTotalReportedHours],[CumConsiderableHours] FROM [FinancialResults] where AccountId="+accountId+ " and EmpId="+empId+" and Year="+year+" and Quarter="+quarter+ " and BillableType=2 order by Month desc ";

            FinancialExpandedResults result= this._db.Query<FinancialExpandedResults>(query).SingleOrDefault();
            if (result == null) {
                result = new FinancialExpandedResults();
                result.EmpId = empId;
                result.AccountId = accountId;
            }
            return result;
        }
    }
}




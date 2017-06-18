using ProDashBoard.Data;
using ProDashBoard.Model;
using ProDashBoard.Models;
using ProDashBoard.Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProDashBoard.Api
{
    public class FinancialResultsController : ApiController
    {
        private FinancialResultsRepository repo;
        private FinancialSummaryRepository summaryRepo;
        private TeamMemberRepository teamRepo;
        private AuthorizationRepository authRepo;
        public FinancialResultsController()
        {
            repo = new FinancialResultsRepository();
            summaryRepo = new FinancialSummaryRepository();
            teamRepo = new TeamMemberRepository();
            authRepo = new AuthorizationRepository();
        }
        // POST api/<controller>
        [HttpPost, Route("api/FinancialResults/newAdd")]
        public void Post([FromBody] List<FinancialResults> results)
        {
            Debug.WriteLine("RESULT SIZE " + results.Count);
           // repo.add(results);

            if (results.Count != 0)
            {
                List<int> empIds = repo.getTimeReportedEmpIdForQuarter(results[0].Year, results[0].Quarter, results[0].AccountId);
                foreach (FinancialResults result in results)
                {
                    empIds.Remove(result.EmpId);

                    //repo.saveData(result);

                }

                foreach (int empId in empIds)
                {
                    TeamMembers member = teamRepo.Get(empId);
                    if (member != null)
                    {
                        FinancialResults tempResult = new FinancialResults();
                        tempResult.EmpId = empId;
                        tempResult.EmpName = member.MemberName;
                        tempResult.AccountId = results[0].AccountId;
                        tempResult.AccountName = results[0].AccountName;
                        tempResult.Month = results[0].Month;
                        tempResult.Quarter = results[0].Quarter;
                        tempResult.Year = results[0].Year;
                        Console.WriteLine(tempResult.EmpName);
                        //  repo.saveData(tempResult);
                    }
                }


            }

            //Calculate Financial Summary
            double expectedHours = 0;
            double actualHours = 0;
            double coveredBillableHours = 0;
            foreach (FinancialResults result in results)
            {
                if (result.BillableType == Constants.FULLTIMEBILLABLE)
                {
                    // The below commented code compares individual allocation with its billable hours

                    //expectedHours += result.AllocatedHours;

                    //double tempActualHours = 0;
                    //if (result.AllocatedHours < result.BillableHours)
                    //{
                    //    tempActualHours = result.AllocatedHours;
                    //}
                    //else
                    //{
                    //    tempActualHours = result.BillableHours;
                    //}
                    //actualHours += tempActualHours;

                    expectedHours += result.AllocatedHours;
                    actualHours += result.BillableHours;
                    coveredBillableHours += result.BillableHours;
                }
            }

            if (results.Count != 0)
            {
                if (actualHours > expectedHours)
                {
                   // actualHours = expectedHours;
                }

                string strDate = "01/" + results[0].Month + "/" + results[0].Year;

                //String strDate = "01/12/2014";
                DateTime dateTime = new DateTime(results[0].Year, results[0].Month, 1);
                //DateTime dateTime = DateTime.ParseExact(strDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                dateTime = dateTime.AddMonths(-1);


                FinancialSummary summary = summaryRepo.getSelectedMonthSummary(dateTime.Year, results[0].AccountId, results[0].Quarter);

                FinancialSummary savingSummary = new FinancialSummary();
                savingSummary.Month = results[0].Month;
                savingSummary.Year = results[0].Year;
                savingSummary.Quarter = results[0].Quarter;
                savingSummary.AccountId = results[0].AccountId;
                savingSummary.AccountName = results[0].AccountName;
                savingSummary.MonthName = getMonth(results[0].Month);

                if (summary != null)
                {
                    double finalAllocatedHours = expectedHours + summary.ExpectedHours;
                    double finalActualHours = actualHours + summary.ActualHours;
                    double finalCoveredBillableHours = coveredBillableHours + summary.coveredBillableHours;
                    finalActualHours = finalCoveredBillableHours;
                    if (finalCoveredBillableHours > finalAllocatedHours) {
                        finalActualHours = finalAllocatedHours;
                    }
                    
                    savingSummary.ExpectedHours = finalAllocatedHours;
                    savingSummary.ActualHours = finalActualHours;
                    savingSummary.coveredBillableHours = finalCoveredBillableHours;

                }
                else
                {
                    savingSummary.ExpectedHours = expectedHours;
                    Debug.WriteLine("Saving Summary " + expectedHours);
                    savingSummary.ActualHours = actualHours;
                    savingSummary.coveredBillableHours = coveredBillableHours;
                }

                summaryRepo.add(savingSummary);
            }
        }

        [HttpPost, Route("api/FinancialResults/add")]
        public void newPost([FromBody] List<FinancialResults> results)
        {
            Debug.WriteLine("RESULT SIZE " + results.Count);
            //repo.add(results);
            double expectedHours = 0;
            double actualHours = 0;
            double coveredBillableHours = 0;
            if (results.Count != 0)
            {
                List<int> empIds = repo.getTimeReportedEmpIdForQuarter(results[0].Year, results[0].Quarter, results[0].AccountId);
                foreach (FinancialResults result in results)
                {
                    empIds.Remove(result.EmpId);

                    //repo.saveData(result);

                }

                foreach (int empId in empIds)
                {
                    TeamMembers member = teamRepo.Get(empId);
                    if (member != null)
                    {
                        FinancialExpandedResults tempResult = new FinancialExpandedResults();
                        tempResult.EmpId = empId;
                        tempResult.EmpName = member.MemberName;
                        tempResult.AccountId = results[0].AccountId;
                        tempResult.AccountName = results[0].AccountName;
                        tempResult.Month = results[0].Month;
                        tempResult.Quarter = results[0].Quarter;
                        tempResult.Year = results[0].Year;
                        Console.WriteLine(tempResult.EmpName);


                        FinancialExpandedResults empFinancialResult= repo.getIndividualEmpDataForLastMonthInQuarter(results[0].AccountId, empId, results[0].Year, results[0].Quarter);
                        if (empFinancialResult != null) {
                            expectedHours += empFinancialResult.CumAllocatedHours;
                            actualHours += empFinancialResult.BillableHours;
                            coveredBillableHours += empFinancialResult.CumConsiderableHours;
                        }
                        //  repo.saveData(tempResult);
                    }
                }


            }

            //Calculate Financial Summary
            
            List<FinancialExpandedResults> newResultList = new List<FinancialExpandedResults>();
            foreach (FinancialResults result in results)
            {
                FinancialExpandedResults lastMonthresult= repo.getIndividualEmpDataForLastMonthInQuarter(result.AccountId, result.EmpId, result.Year, result.Quarter);
                FinancialExpandedResults currentResult = result;
                
                if (result.BillableType == Constants.FULLTIMEBILLABLE)
                {
                    //results
                    currentResult.CumAllocatedHours = lastMonthresult.CumAllocatedHours + currentResult.AllocatedHours;
                    currentResult.CumBillableHours = lastMonthresult.CumBillableHours + currentResult.BillableHours;
                    currentResult.CumTotalReportedHours = lastMonthresult.CumTotalReportedHours + currentResult.TotalReportedHours;
                    if (currentResult.CumBillableHours > currentResult.CumAllocatedHours)
                    {
                        currentResult.CumConsiderableHours = currentResult.CumAllocatedHours;
                    }
                    else
                    {
                        currentResult.CumConsiderableHours = currentResult.CumBillableHours;
                    }

                    }
                else if (result.BillableType == Constants.NONBILLABLE || result.BillableType==Constants.HOURLYBILLABLE) {

                    currentResult.CumAllocatedHours = lastMonthresult.CumAllocatedHours;
                    currentResult.CumBillableHours = lastMonthresult.CumBillableHours;
                    currentResult.CumTotalReportedHours = lastMonthresult.CumTotalReportedHours;
                    currentResult.CumConsiderableHours = lastMonthresult.CumConsiderableHours;
                }

                newResultList.Add(currentResult);
                //summary
                expectedHours += currentResult.CumAllocatedHours;
                actualHours += currentResult.CumBillableHours;
                coveredBillableHours += currentResult.CumConsiderableHours;
            }

            if (results.Count != 0)
            {
                repo.add(newResultList);
                
                FinancialSummary savingSummary = new FinancialSummary();
                savingSummary.Month = results[0].Month;
                savingSummary.Year = results[0].Year;
                savingSummary.Quarter = results[0].Quarter;
                savingSummary.AccountId = results[0].AccountId;
                savingSummary.AccountName = results[0].AccountName;
                savingSummary.MonthName = getMonth(results[0].Month);

                
                savingSummary.ExpectedHours = expectedHours;
                    Debug.WriteLine("Saving Summary " + expectedHours);
                    savingSummary.ActualHours = actualHours;
                    savingSummary.coveredBillableHours = coveredBillableHours;
                

                summaryRepo.add(savingSummary);
            }
        }
        public string getMonth(int monthNum)
        {
            string monthName = "";
            int month = monthNum;
            switch (month)
            {
                case 1: monthName = "January"; break;
                case 2: monthName = "February"; break;
                case 3: monthName = "March"; break;
                case 4: monthName = "April"; break;
                case 5: monthName = "May"; break;
                case 6: monthName = "June"; break;
                case 7: monthName = "July"; break;
                case 8: monthName = "August"; break;
                case 9: monthName = "September"; break;
                case 10: monthName = "October"; break;
                case 11: monthName = "November"; break;
                case 12: monthName = "December"; break;

            }
            return monthName;
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }

        [HttpGet, Route("api/FinancialResults/getSelectedFinancialResults/{Year}/{Quarter}/{AccountId}")]
        public HttpResponseMessage getSelectedFinancialResults(int Year, int Quarter, int AccountId)
        {
            if (authRepo.isAuthorized(AccountId))
            {
                return Request.CreateResponse(HttpStatusCode.OK, repo.getSelectedFinancialResults(Year, Quarter, AccountId));
            }
            else {
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }
            
        }

        [HttpPut, Route("api/FinancialResults/update")]
        public void updateFinancialResults([FromBody] List<FinancialResults> results)
        {
            Debug.WriteLine("RESULT SIZE " + results.Count);
            int didExistingResultesDeleted = 0;
            int didExistingSummaryDeleted = 0;
            if (results.Count != 0)
            {
                didExistingResultesDeleted = repo.deleteFinancialResults(results[0].AccountId, results[0].Year, results[0].Quarter, results[0].Month);
                didExistingSummaryDeleted = summaryRepo.deleteExistingSummary(results[0].AccountId, results[0].Year, results[0].Quarter, results[0].Month);
            }

            if (didExistingResultesDeleted > 0 & didExistingSummaryDeleted>0)
            {
                //  repo.add(results);
                double expectedHours = 0;
                double actualHours = 0;
                double coveredBillableHours = 0;
                if (results.Count != 0)
                {
                    List<int> empIds = repo.getTimeReportedEmpIdForQuarter(results[0].Year, results[0].Quarter, results[0].AccountId);
                    foreach (FinancialResults result in results)
                    {
                        empIds.Remove(result.EmpId);

                        //repo.saveData(result);

                    }

                    foreach (int empId in empIds)
                    {
                        TeamMembers member = teamRepo.Get(empId);
                        if (member != null)
                        {
                            FinancialExpandedResults tempResult = new FinancialExpandedResults();
                            tempResult.EmpId = empId;
                            tempResult.EmpName = member.MemberName;
                            tempResult.AccountId = results[0].AccountId;
                            tempResult.AccountName = results[0].AccountName;
                            tempResult.Month = results[0].Month;
                            tempResult.Quarter = results[0].Quarter;
                            tempResult.Year = results[0].Year;
                            Console.WriteLine(tempResult.EmpName);
                            //  repo.saveData(tempResult);

                            FinancialExpandedResults empFinancialResult = repo.getIndividualEmpDataForLastMonthInQuarter(results[0].AccountId, empId, results[0].Year, results[0].Quarter);
                            if (empFinancialResult != null)
                            {
                                expectedHours += empFinancialResult.CumAllocatedHours;
                                actualHours += empFinancialResult.BillableHours;
                                coveredBillableHours += empFinancialResult.CumConsiderableHours;
                            }
                        }
                    }


                }

                //Calculate Financial Summary
                
                List<FinancialExpandedResults> newResultList = new List<FinancialExpandedResults>();
                foreach (FinancialResults result in results)
                {
                    FinancialExpandedResults lastMonthresult = repo.getIndividualEmpDataForLastMonthInQuarter(result.AccountId, result.EmpId, result.Year, result.Quarter);
                    FinancialExpandedResults currentResult = result;



                    if (result.BillableType == Constants.FULLTIMEBILLABLE)
                    {
                        //results
                        currentResult.CumAllocatedHours = lastMonthresult.CumAllocatedHours + currentResult.AllocatedHours;
                        currentResult.CumBillableHours = lastMonthresult.CumBillableHours + currentResult.BillableHours;
                        currentResult.CumTotalReportedHours = lastMonthresult.CumTotalReportedHours + currentResult.TotalReportedHours;
                        if (currentResult.CumBillableHours > currentResult.CumAllocatedHours)
                        {
                            currentResult.CumConsiderableHours = currentResult.CumAllocatedHours;
                        }
                        else
                        {
                            currentResult.CumConsiderableHours = currentResult.CumBillableHours;
                        }

                    }
                    else if (result.BillableType == Constants.NONBILLABLE || result.BillableType == Constants.HOURLYBILLABLE)
                    {

                        currentResult.CumAllocatedHours = lastMonthresult.CumAllocatedHours;
                        currentResult.CumBillableHours = lastMonthresult.CumBillableHours;
                        currentResult.CumTotalReportedHours = lastMonthresult.CumTotalReportedHours;
                        currentResult.CumConsiderableHours = lastMonthresult.CumConsiderableHours;
                    }

                    newResultList.Add(currentResult);
                    //summary
                    expectedHours += currentResult.CumAllocatedHours;
                    actualHours += currentResult.CumBillableHours;
                    coveredBillableHours += currentResult.CumConsiderableHours;
                }

                if (results.Count != 0)
                {
                    repo.add(newResultList);

                    FinancialSummary savingSummary = new FinancialSummary();
                    savingSummary.Month = results[0].Month;
                    savingSummary.Year = results[0].Year;
                    savingSummary.Quarter = results[0].Quarter;
                    savingSummary.AccountId = results[0].AccountId;
                    savingSummary.AccountName = results[0].AccountName;
                    savingSummary.MonthName = getMonth(results[0].Month);


                    savingSummary.ExpectedHours = expectedHours;
                    Debug.WriteLine("Saving Summary " + expectedHours);
                    savingSummary.ActualHours = actualHours;
                    savingSummary.coveredBillableHours = coveredBillableHours;


                    summaryRepo.add(savingSummary);
                }




            }
        }
    }
}




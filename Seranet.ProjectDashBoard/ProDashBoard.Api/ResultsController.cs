using ProDashBoard.Data;
using ProDashBoard.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using System.Security.Principal;

using System.Collections;

namespace ProDashBoard.Api
{
    public class ResultsController : ApiController
    {

        private ResultsRepository repo;
        private QuestionRepository questionrepo;
        private TeamSatisfactionEmployeeSummaryRepository teamSatEmployeeSummaryRepo;
        private SummaryRepository summaryRepo;
        private AuthorizationRepository authRepo;
        private static int count;
        
        public ResultsController()
        {
            repo = new ResultsRepository();
            questionrepo = new QuestionRepository();
            teamSatEmployeeSummaryRepo = new TeamSatisfactionEmployeeSummaryRepository();
            summaryRepo = new SummaryRepository();
            authRepo = new AuthorizationRepository();
            count = 0;
            
        }

        [HttpGet, Route("api/Results")]

        public List<SatisfactionResults> Get()
        {
            return repo.Get();
        }

        [HttpGet, Route("api/Results/{id}")]

        public SatisfactionResults Get(int id)
        {
           
            SatisfactionResults questions = null;
            if (repo.Get(id) != null)
            {
                questions = repo.Get(id);
            }
            return questions;
            
        }

        [HttpGet, Route("api/Results/getSelectedResults/{id}/{year}/{quarter}")]

        public HttpResponseMessage getSelectedResults(int id,int year,int quarter)
        {
            List<List<Object[]>> returnData=new List<List<object[]>>();
            bool rights = authRepo.getAdminRights() || authRepo.getTeamLeadRights(id);
            bool b = authRepo.getAccountRights(id);
            Debug.WriteLine("SummaryAuth " + b + " " + authRepo.getAdminRights() + " " + authRepo.getTeamLeadRights(id));
            if (authRepo.isAuthorized(id)) {
                returnData = repo.getSelectedResults(id, year, quarter);
                return Request.CreateResponse(HttpStatusCode.OK, returnData);
            } else {
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }
            
        }

        [HttpGet, Route("api/Results/getReviewData/{projectId}/{year}/{quarter}/{employeeId}")]
        public HttpResponseMessage getReviweData(int projectId, int year, int quarter, int employeeId) {
            List<Object[]> returnData = new List<object[]>();
            bool rights=authRepo.getAdminRights()||authRepo.getTeamLeadRights(projectId);
            bool b = authRepo.getLoggedInUserAuthentication(employeeId);
            Debug.WriteLine("Rights " + authRepo.getAdminRights()+" "+ authRepo.getTeamLeadRights(projectId));
            if (rights)
            {
                returnData = repo.getReviweData(projectId, year, quarter, employeeId);
                return Request.CreateResponse(HttpStatusCode.OK, returnData);
            }
            else if (b)
            {
                returnData = repo.getReviweData(projectId, year, quarter, employeeId);
                return Request.CreateResponse(HttpStatusCode.OK, returnData);
            }
            else {
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }
            
        }

        //$scope.tempArray.push($scope.empCombo);
        //       $scope.tempArray.push($scope.accountCombo);
        //       $scope.tempArray.push($scope.teamCombo);
        //       $scope.tempArray.push($scope.yearCombo);
        //       $scope.tempArray.push($scope.quarterCombo);
        //       $scope.tempArray.push($scope.Questions[y].Id);
        //       $scope.tempArray.push($scope.rateArray[$scope.Questions[y].Id]);
        //       $scope.tempArray.push($scope.commentArray[$scope.Questions[y].Id]);

        //[HttpPost, Route("api/Results/add")]
        //public int add([FromBody] string nam)
        //{

        [HttpPost, Route("api/Results/add")]
        //[ResponseType(typeof(String))]
        public int add([FromBody] string text)
        {

            String[] outputArray= text.Split('~');
            int singleRating=0;
        List<Questions> displayingQuestions = questionrepo.getDisplayingQuestions(1);
            for (int i = 1; i < outputArray.Length;i++) {
                SatisfactionResults satResults = new SatisfactionResults();
                Debug.WriteLine("gdfg " + outputArray[i]);
                
                String[] resultedArray=outputArray[i].Split('|');
                satResults.MemberId = Convert.ToInt32(resultedArray[0]);
                satResults.AccountId = Convert.ToInt32(resultedArray[1]);
                satResults.ProjectId = Convert.ToInt32(resultedArray[2]);
                satResults.Year = Convert.ToInt32(resultedArray[3]);
                satResults.Quarter = Convert.ToInt32(resultedArray[4]);
                satResults.QuestionId = Convert.ToInt32(resultedArray[5]);
                if (resultedArray[6].Equals("undefined"))
                {
                    satResults.Answer = "0";
                    satResults.Comment = resultedArray[7];
                }
                else {
                    satResults.Answer= resultedArray[6];
                    satResults.Comment = resultedArray[7];
                }
                repo.add(satResults);
                
                foreach (Questions qu in displayingQuestions) {
                    if (qu.Id == satResults.QuestionId) {
                        singleRating = singleRating + Convert.ToInt32(satResults.Answer);
                    }
                }

                
                
            }
            double employeeSummary = (double)((double)singleRating / (displayingQuestions.Count));
            Debug.WriteLine("SingleSummary " + employeeSummary);
            TeamSatisfactionEmployeeSummary teamSatisfactionEmpSummary = new TeamSatisfactionEmployeeSummary();
            String[] newResultedArray = outputArray[1].Split('|');
            teamSatisfactionEmpSummary.empId = Convert.ToInt32(newResultedArray[0]);
            teamSatisfactionEmpSummary.AccountId = Convert.ToInt32(newResultedArray[1]);
            teamSatisfactionEmpSummary.Year = Convert.ToInt32(newResultedArray[3]);
            teamSatisfactionEmpSummary.Quarter = Convert.ToInt32(newResultedArray[4]);
            teamSatisfactionEmpSummary.Rating = employeeSummary;
            teamSatEmployeeSummaryRepo.add(teamSatisfactionEmpSummary);
            double summaryAverage=teamSatEmployeeSummaryRepo.getSelectedAccountSummaryTotal(Convert.ToInt32(newResultedArray[1]), Convert.ToInt32(newResultedArray[3]), Convert.ToInt32(newResultedArray[4]));

            int affectedCount=summaryRepo.updateSelectedSummary(Convert.ToInt32(newResultedArray[1]), Convert.ToInt32(newResultedArray[3]), Convert.ToInt32(newResultedArray[4]), summaryAverage);
            //SatisfactionResults sat = new SatisfactionResults();
            //sat.MemberId = empId;
            //sat.AccountId = accId;
            //sat.Year = year;
            //sat.ProjectId = teamId;
            //sat.Quarter = quarter;
            //sat.QuestionId = qId;
            //if (rate != -1)
            //{
            //    sat.Answer = rate.ToString();
            //    sat.Comment = comment;
            //}
            //else {
            //    sat.Comment = comment;
            //    sat.Answer = "0";
            //}

            //int returnRowData=repo.add(sat);
            //count++;
            //Debug.WriteLine(count+" fdsf " + empId + " " + accId + " " + year + " " + quarter + " " + qId + " " + rate + " " + comment);
            //List<Questions> displayingQuestions=questionrepo.getDisplayingQuestions(1);
            //foreach (Questions qu in displayingQuestions) {
            //    if (qu.Id == sat.QuestionId) {
            //        singleRating = singleRating + Convert.ToInt32(sat.Answer);
            //    }
            //}
            //if (count == questionLength) {

            //    Debug.WriteLine("ResultEntered "+singleRating);
            //    double finalEmployeeRating = singleRating / questionLength;
            //    TeamSatisfactionEmployeeSummary teamSatisfactionEmpSummary = new TeamSatisfactionEmployeeSummary();
            //    teamSatisfactionEmpSummary.empId = empId;
            //    teamSatisfactionEmpSummary.AccountId = accId;
            //    teamSatisfactionEmpSummary.Year = year;
            //    teamSatisfactionEmpSummary.Quarter = quarter;
            //    teamSatisfactionEmpSummary.Rating = finalEmployeeRating;
            //    teamSatEmployeeSummaryRepo.add(teamSatisfactionEmpSummary);
            //}
            return affectedCount;


        }
    }
}

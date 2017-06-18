using Newtonsoft.Json;
using ProDashBoard.Data;
using ProDashBoard.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace ProDashBoard.Api
{
    [Authorize]
    public class ProcessComplianceResultsController : ApiController
    {
        private ProcessComplianceResultsRepository repo;
        private ProcessComplianceSummaryRepository summaryRepo;

        public ProcessComplianceResultsController()
        {
            repo = new ProcessComplianceResultsRepository();
            summaryRepo = new ProcessComplianceSummaryRepository();
        }

        [HttpGet, Route("api/ProcessCompliance/getSelectedProjectResults/{accountId}/{projectId}/{year}/{quarter}")]
        public List<ProcessComplianceResults> getSelectedProjectResults(int accountId, int projectId,int year,int quarter)
        {
            return repo.getSelectedProjectResults(accountId,projectId,year,quarter);
        }

        
        [HttpPost, Route("api/ProcessCompliance/add")]
        
        public int add([FromBody] string resulteddata)
        {
            
            Debug.WriteLine("Retr " + resulteddata);
            string[] outputArray = resulteddata.Split('|');
            ProcessComplianceResults results = new ProcessComplianceResults();
            ProcessComplianceSummary summary = new ProcessComplianceSummary();
            results.AccountId = Convert.ToInt32(outputArray[1]);
            results.ProjectId = Convert.ToInt32(outputArray[0]);

            Debug.WriteLine("Account "+ results.AccountId+" Project"+results.ProjectId);

            results.Year = Convert.ToInt32(outputArray[2]);
            results.Quarter = Convert.ToInt32(outputArray[3]);

            summary.AccountId = Convert.ToInt32(outputArray[1]);
            summary.ProjectId = Convert.ToInt32(outputArray[0]);
            summary.Year = Convert.ToInt32(outputArray[2]);
            summary.Quarter = Convert.ToInt32(outputArray[3]);
            summary.Rating = Convert.ToDouble(outputArray[4]);
            summary.ProcessVersion =Convert.ToString(outputArray[5]);

            string[] parameterArray = outputArray[6].Split(':');
            Debug.WriteLine("gd " + parameterArray[0] + " " + parameterArray.Length);
            for (int i = 0; i < (parameterArray.Length - 1); i++)
            {
                string val = parameterArray[i];
                string id = val.Split('-')[0];
                string quality = val.Split('-')[1];
                results.QualityParameterId = Convert.ToInt32(id);
                results.Rating = quality;
                repo.add(results);
                Debug.WriteLine("Qualities " + id + " " + quality);
            }
            int x=summaryRepo.add(summary);

            return x;
        }
    }
}

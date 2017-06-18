using ProDashBoard.Data;
using ProDashBoard.Model.Repository;
using ProDashBoard.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Web;
using spClient = Microsoft.SharePoint.Client;

namespace ProDashBoard.Repository
{
    public class RiskManagementRepository : IRiskManagementRepository
    {
        private readonly IDbConnection _db;
        private CommonDataRepository commonRepo;
        public RiskManagementRepository()
        {
            _db = new SqlConnection(ConfigurationManager.ConnectionStrings["DashBoard1"].ConnectionString);
            commonRepo = new CommonDataRepository();
        }

        public spClient.ListItemCollection getSharePointData(string subProjectRiskUrl, string query)
        {
            spClient.ListItemCollection collectionList = null;
            try
            {
                string[] validUrlApi = subProjectRiskUrl.Split(new string[] { "/Lists/" }, StringSplitOptions.None);
                string newriskUrl = subProjectRiskUrl;
                if (validUrlApi.Length != 0)
                {
                    newriskUrl = validUrlApi[0] + "/";
                }
                using (spClient.ClientContext ctx = new spClient.ClientContext(newriskUrl))
                {
                    
                    var passWord = new SecureString();
                    foreach (var c in "intel@123") passWord.AppendChar(c);
                    ctx.Credentials = new spClient.SharePointOnlineCredentials("systemriskreview@99x.lk", passWord);

                    spClient.Web myWeb = ctx.Web;
                    spClient.List proList = myWeb.Lists.GetByTitle("Risk List");
                    spClient.CamlQuery myQuery = new spClient.CamlQuery();
                    myQuery.ViewXml = query;
                    collectionList = proList.GetItems(myQuery);
                    ctx.Load(collectionList);
                    ctx.ExecuteQuery();

                }
            }
            catch (Exception e)
            {
                collectionList = null;
            }
            return collectionList;
        }

        public List<RiskData> getTotalRisksForSelectAccountSubProjects(List<Project> subProjects)
        {

            List<RiskData> totalsubProjectRisks = new List<RiskData>();
            Boolean isAnyRiskDataAvailable = false;
            foreach (Project subproject in subProjects)
            {
                CommonData commonData=commonRepo.getSelectedProjectCommonData(subproject.Id);
                if (commonData != null && commonData.RiskPageUrl != null)
                {
                    string query = "<View><Query><OrderBy><FieldRef Name = 'RiskValue' Ascending = 'FALSE' /></OrderBy></Query><RowLimit>1</RowLimit></View> ";
                    spClient.ListItemCollection collectionList = getSharePointData(commonData.RiskPageUrl, query);
                    if (collectionList != null)
                    {
                        RiskData riskData = new RiskData();
                        int riskValueSimilarCount = 0;
                        int riskValue = 0;
                        try
                        {
                            riskValue = Convert.ToInt32(((spClient.ListItem)collectionList[0])["RiskValue"].ToString());
                        }
                        catch (Exception e)
                        {
                            riskValue = 0;
                        }
                        foreach (spClient.ListItem item in collectionList)
                        {
                            //    if (maxValue == Convert.ToInt32(item["RiskValue"].ToString()))
                            //    {
                            riskValueSimilarCount++;
                            riskData.riskTitle = item["Title"].ToString();
                            riskData.riskValue = Convert.ToInt32(item["RiskValue"].ToString());
                            riskData.riskImpact = item["RiskImpact"].ToString();
                            riskData.riskProbability = item["RiskProbability"].ToString();
                            riskData.subProject = subproject;
                            if (commonData != null)
                            {
                                riskData.riskUrl = commonData.RiskPageUrl;
                            }
                            Debug.WriteLine(item["Title"].ToString() + " " + riskData.riskValue);
                            //}
                            //else {
                            //    break;
                            //}
                        }
                        riskData.riskValueSimilarCount = riskValueSimilarCount;
                        if (riskValue != 0)
                        {
                            isAnyRiskDataAvailable = true;
                            totalsubProjectRisks.Add(riskData);
                        }
                        else
                        {
                            RiskData optionalRiskData = new RiskData();
                            optionalRiskData.subProject = subproject;
                            optionalRiskData.riskValue = -1;
                            totalsubProjectRisks.Add(optionalRiskData);
                        }
                    }
                    else {
                        RiskData riskData = new RiskData();
                        riskData.subProject = subproject;
                        riskData.riskValue = -1;
                        totalsubProjectRisks.Add(riskData);
                    }
                }
                else {
                    RiskData riskData = new RiskData();
                    riskData.subProject = subproject;
                    riskData.riskValue = -1;
                    totalsubProjectRisks.Add(riskData);
                }
            }
            if (!isAnyRiskDataAvailable) {
                totalsubProjectRisks = new List<RiskData>();
            }
            Debug.WriteLine("-------------");
            Debug.WriteLine(totalsubProjectRisks.Count);
            return totalsubProjectRisks;
        }
    }
}


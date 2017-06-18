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
using spClient = Microsoft.SharePoint.Client;
using System.Net;
using System.Security;

namespace ProDashBoard.Data
{
    public class CommonDataRepository : ICommonDataRepository
    {
        private readonly IDbConnection _db;

        public CommonDataRepository()
        {
            _db = new SqlConnection(ConfigurationManager.ConnectionStrings["DashBoard1"].ConnectionString);
        }
        public CommonData getSelectedProjectCommonData(int projectid)
        {
            //using (spClient.ClientContext ctx = new spClient.ClientContext("https://99xtech.sharepoint.com/departments/Delivery_stage/Project_Test2/")) {
            //    var passWord = new SecureString();
            //    foreach (var c in "intel@123") passWord.AppendChar(c);
            //    ctx.Credentials = new spClient.SharePointOnlineCredentials("systemriskreview@99x.lk", passWord);

            //    spClient.Web myWeb = ctx.Web;
            //    spClient.List proList = myWeb.Lists.GetByTitle("Risk List");
            //    spClient.CamlQuery myQuery = new spClient.CamlQuery();
            //    myQuery.ViewXml = "<View><RowLimit>100</RowLimit></View>";
            //    spClient.ListItemCollection collectionList=proList.GetItems(myQuery);
            //    ctx.Load(collectionList);
            //    ctx.ExecuteQuery();

            //    foreach (spClient.ListItem itm in collectionList) {
            //        Debug.WriteLine(itm["Title"].ToString());
            //    }

            //} ;
            CommonData data=_db.Query<CommonData>("SELECT * FROM [CommonData] WHERE ProjectId = '" +projectid+ "'").SingleOrDefault();
            //Debug.WriteLine("PCLINK "+data.ProcessComplianceLink+" "+ data.Id+" "+data.ProjectId);
            return data;
        }

        public int update(CommonData commonData)
        {
            int datarows = 0;

            //datarows = this._db.Execute("UPDATE Account set [AccountName]=@AccountName,[AccCode]=@AccCode,[Availability]=@Availability,[AccountOwner]=@AccountOwner,[Description]=@Description WHERE [Id]=@Id",
            //    new { AccountName = account.AccountName, AccCode = account.AccCode, Availability = account.Availability, AccountOwner = account.AccountOwner, Description = account.Description, Id = account.Id });

            datarows = this._db.Execute("UPDATE CommonData set [WikiPageLink]=@WikiPageLink, [ConfluencePageId]=@ConfluencePageId, [RiskPageUrl]=@RiskPageUrl WHERE [ProjectId]=@ProjectId",
                new { WikiPageLink = commonData.WikiPageLink, ConfluencePageId=commonData.ConfluencePageId, RiskPageUrl = commonData.RiskPageUrl, ProjectId = commonData.ProjectId });
            if (datarows == 0) {
                add(commonData);
            }
            return datarows;
        }

        public int add(CommonData commonData)
        {
            int datarows = 0;

            datarows = this._db.Execute(@"INSERT CommonData([ProjectId],[WikiPageLink],[ConfluencePageId],[RiskPageUrl]) values (@ProjectId, @WikiPageLink,@ConfluencePageId,@RiskPageUrl)",
                new { ProjectId=commonData.ProjectId, WikiPageLink =commonData.WikiPageLink, ConfluencePageId=commonData.ConfluencePageId, RiskPageUrl = commonData.RiskPageUrl});

            return datarows;
        }
    }
}

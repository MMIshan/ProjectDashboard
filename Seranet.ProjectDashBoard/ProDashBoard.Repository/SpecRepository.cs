using ProDashBoard.Model.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Dapper;
using System.Diagnostics;

namespace ProDashBoard.Repository
{
    public class SpecRepository : ISpecRepository
    {
        private readonly IDbConnection _db;
        public SpecRepository() {
            _db = new SqlConnection(ConfigurationManager.ConnectionStrings["Spec"].ConnectionString);
        }

        public int getSpecLevel(string projectIdToSpec) {
            string query = @"
                            select ((select 'Claimed' = count(*) from practice where Level_Id=le.id and id in (select Practice_Id from claim where Practice_Id in(select id from Practice where SubArea_Id in(select id from SubArea where Area_Id=1)) ))-(select 'Claimed' = count(*) from practice where Level_Id=le.id and id in (select Practice_Id from claim where project_id =(select id from Project where ProjetId=@projectCode) and (status ='1' or status ='3') and Practice_Id in(select id from Practice where SubArea_Id in(select id from SubArea where Area_Id=1)) ) ) ) from Level le group by (id)
                            
                            select ((select 'Claimed' = count(*) from practice where Level_Id=le.id and id in (select Practice_Id from claim where Practice_Id in(select id from Practice where SubArea_Id in(select id from SubArea where Area_Id=2)) ))-(select 'Claimed' = count(*) from practice where Level_Id=le.id and id in (select Practice_Id from claim where project_id =(select id from Project where ProjetId=@projectCode) and (status ='1' or status ='3') and Practice_Id in(select id from Practice where SubArea_Id in(select id from SubArea where Area_Id=2)) ) ) ) from Level le group by (id)
                            
                            select ((select 'Claimed' = count(*) from practice where Level_Id=le.id and id in (select Practice_Id from claim where Practice_Id in(select id from Practice where SubArea_Id in(select id from SubArea where Area_Id=3)) ))-(select 'Claimed' = count(*) from practice where Level_Id=le.id and id in (select Practice_Id from claim where project_id =(select id from Project where ProjetId=@projectCode) and (status ='1' or status ='3') and Practice_Id in(select id from Practice where SubArea_Id in(select id from SubArea where Area_Id=3)) ) ) ) from Level le group by (id)
                            
                            select ((select 'Claimed' = count(*) from practice where Level_Id=le.id and id in (select Practice_Id from claim where Practice_Id in(select id from Practice where SubArea_Id in(select id from SubArea where Area_Id=4)) ))-(select 'Claimed' = count(*) from practice where Level_Id=le.id and id in (select Practice_Id from claim where project_id =(select id from Project where ProjetId=@projectCode) and (status ='1' or status ='3') and Practice_Id in(select id from Practice where SubArea_Id in(select id from SubArea where Area_Id=4)) ) ) ) from Level le group by (id)
                            ";
            List<int> area1Array=new List<int>();
            List<int> area2Array= new List<int>();
            List<int> area3Array = new List<int>();
            List<int> area4Array = new List<int>();
            List<List<int>> allAreaArrays = new List<List<int>>();
            
            using (var multi = _db.QueryMultiple(query, new { projectCode = projectIdToSpec }))
            {
                area1Array = multi.Read<int>().ToList();
                area2Array = multi.Read<int>().ToList();
                area3Array = multi.Read<int>().ToList();
                area4Array = multi.Read<int>().ToList();
                allAreaArrays.Add(area1Array);
                allAreaArrays.Add(area2Array);
                allAreaArrays.Add(area3Array);
                allAreaArrays.Add(area4Array);

            }
            List<int> levels = new List<int>();
            foreach (List<int> tempArray in allAreaArrays)
            {
                int areaLevel = 0;
                try
                {
                    if (tempArray.ElementAt(0) == 0)
                    {
                        areaLevel = 1;
                    }
                    if (tempArray.ElementAt(1) == 0)
                    {
                        areaLevel = 2;
                    }
                    if (tempArray.ElementAt(2) == 0)
                    {
                        areaLevel = 3;
                    }
                }
                catch (Exception e) {
                    Debug.WriteLine("Error while checking areaLevel");
                }
                levels.Add(areaLevel);
            }
            levels.Sort();
            Debug.WriteLine("SPEC " + levels.ElementAt(0));
            if (levels.Count != 0)
            {
                return levels.ElementAt(0);
            }
            else {
                return -1;
            }
            
            
        }

        public int getSpecProjectId(string projectCode) {
            string query = "select id from Project where ProjetId='"+projectCode+ "'";
            int projectId=_db.Query<int>(query).SingleOrDefault();
            if (projectId == 0)
            {
                return -1;
            }
            else {
               return projectId;
            }
        }
    }
}


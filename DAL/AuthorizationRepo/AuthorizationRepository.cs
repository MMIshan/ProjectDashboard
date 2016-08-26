using ProDashBoard.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace ProDashBoard.DAL.AuthorizationRepo
{
    public class AuthorizationRepository : IAuthorizationRepository
    {
        private TeamMemberRepository teamMemberRepo;
        private EmployeeProjectsRepository empProjectRepo;
        private static TeamMembers loggedInuser;

        public AuthorizationRepository() {
            teamMemberRepo = new TeamMemberRepository();
            empProjectRepo = new EmployeeProjectsRepository();
            loggedInuser= teamMemberRepo.getSelectedEmployee(getUsername());
        }
        public string getUsername()
        {
            WindowsIdentity identity = System.Web.HttpContext.Current.Request.LogonUserIdentity;
            String username = (identity.Name).Split('\\')[1].ToLower();
            Debug.WriteLine("UserName " + username);
            return username;
        }

        public bool getAdminRights()
        {
            TeamMembers tm = teamMemberRepo.getSelectedEmployee(getUsername());
            if (tm != null)
            {
                return tm.AdminRights;
            }
            else
            {
                return false;
            }
        }

        public bool getLoggedInUserAuthentication(int id)
        {
          
            if (loggedInuser != null)
            {
                if (loggedInuser.Id == id)
                {
                    return true;
                }
                else {
                    return false;
                }
            }
            else {
                return false;
            }
        }

        public bool getTeamLeadRights(int accountId)
        {
            if (loggedInuser != null)
            {
                EmployeeProjects emp = empProjectRepo.getLoggedInUserLeadRights(accountId, loggedInuser.Id, 1);

                if (emp != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else {
                return false;
            }
        }

        public bool getAccountRights(int accountId)
        {
            if (loggedInuser != null)
            {
                EmployeeProjects emp = empProjectRepo.getLoggedInUserLeadRights(accountId, loggedInuser.Id,0);

            if (emp != null)
            {
                return true;
            }
            else
            {
                return false;
            }
            }
            else
            {
                return false;
            }
        }

        public bool isAuthenticated(int accountId)
        {
          bool b= getAdminRights() || getTeamLeadRights(accountId) || getAccountRights(accountId);
            return b;
        }
    }
}
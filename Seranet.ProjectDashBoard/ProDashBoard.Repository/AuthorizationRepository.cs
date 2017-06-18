using ProDashBoard.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Web;
using ProDashBoard.Model.Repository;

namespace ProDashBoard.Data
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
            Debug.WriteLine(identity.Name + "  UserName  " + username);
            return username;
        }

        public TeamMembers getLoggedInUser() {
            return teamMemberRepo.getSelectedEmployee(getUsername());
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
                int doesAccountOwnerExists = empProjectRepo.isAnAccountOwner(accountId, loggedInuser.Id);
                if (emp != null || (doesAccountOwnerExists==1))
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

        public bool getTeamLeadRightsForAnyAccount()
        {
            if (loggedInuser != null)
            {
                EmployeeProjects emp = empProjectRepo.getLoggedInUserLeadRightsForAnyAccount(loggedInuser.Id);
                int doesAccountOwnerExists = empProjectRepo.isAnyAccountOwner(loggedInuser.Id);
                if (emp != null || (doesAccountOwnerExists==1))
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

        

        public bool isAuthorized(int accountId)
        {
          bool b= getAdminRights() || getTeamLeadRights(accountId) || getAccountRights(accountId);
            return b;
        }


    }
}



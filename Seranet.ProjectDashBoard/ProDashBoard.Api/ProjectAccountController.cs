using ProDashBoard.Data;
using ProDashBoard.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProDashBoard.Api
{
    public class ProjectAccountController : ApiController
    {
        private AccountRepository repo;
        private TeamMemberRepository tmRepo;
        private AuthorizationRepository authRepo;

        public ProjectAccountController()
        {
            repo = new AccountRepository();
            tmRepo = new TeamMemberRepository();
            authRepo = new AuthorizationRepository();
        }

        [HttpGet, Route("api/Account")]

        public List<Account> Get()
        {
            //List<Account> returnData = repo.Get();
            return repo.Get();
        }

        [HttpGet, Route("api/Account/adminPanelActiveAccounts")]

        public List<Account> getAdminPanelActiveAcounts()
        {
            return repo.getAdminPanelActiveAccounts();
        }

        [HttpGet, Route("api/Account/adminPanelInactiveAccounts")]

        public List<Account> getAdminPanelInactiveAcounts()
        {
            return repo.getAdminPanelInactiveAccounts();
        }

        [HttpGet, Route("api/Account/{id}")]
        public Account Get(int id)
        {
            return repo.Get(id);
        }

        [HttpGet, Route("api/Account/getInacativeAccounts")]
        public List<Account> getInactiveAccounts() {
           return repo.getInactiveAccounts();
        }


        [HttpGet, Route("api/Account/getSpec/{accountid}")]
        public Spec getSpec(int accountid)
        {
            Spec spec = null;
            Debug.WriteLine("gdfgfd" + spec + "gfd");
            if (repo.GetSpec(accountid) != null)
            {

                spec = repo.GetSpec(accountid);
            }

            if (spec != null)
            {
                Debug.WriteLine("Entered");
            }
            else
            {
                Debug.WriteLine("NotEntered");
            }

            return spec;
        }

        [HttpPost, Route("api/Account/add")]
        public int add([FromBody] Account account) {
            return repo.add(account);
        }

        [HttpPut, Route("api/Account/update")]
        public int put(Account account) {
            Debug.WriteLine("Put method Accesses");
            return repo.update(account);
        }

        [HttpPost, Route("api/Account/addIfNotexists")]
        public int addIfNotexists([FromBody] string accountData)
        {
            

            string[] accountArray = accountData.Split('~');
            foreach (string account in accountArray)
            {
                if (account != "undefined")
                {
                    Debug.WriteLine("Account " + account);
                   string userName = account.Split(':')[2];
                    string accountName = account.Split(':')[1];
                    string accountCode = account.Split(':')[0];
                    TeamMembers teamMember = tmRepo.getSelectedEmployee(userName);
                    if (teamMember != null)
                    {
                        Account newAccount = new Account();
                        newAccount.AccCode = accountCode;
                        newAccount.AccountName = accountName;
                        newAccount.AccountOwner = teamMember.Id;
                        newAccount.Availability = true;

                        Account existingAccount = repo.Get(accountCode);
                        if (existingAccount == null) {
                            repo.add(newAccount);
                        }


                        
                    }

                }
            }
            return 0;
        }
    }
}

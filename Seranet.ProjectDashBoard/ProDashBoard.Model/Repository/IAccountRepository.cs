using ProDashBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProDashBoard.Model.Repository
{
    public interface IAccountRepository
    {
        List<Account> Get();
        Account Get(int id);

        Spec GetSpec(int accountId);
        List<Object[]> getAllAccounts();
        List<Account> getInactiveAccounts();
        int add(Account account);

        int update(Account account);
        Account Get(string code);
    }
}

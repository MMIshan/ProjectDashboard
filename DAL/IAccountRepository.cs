using ProDashBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProDashBoard.DAL
{
    interface IAccountRepository
    {
        List<Account> Get();
        Account Get(int id);

        Spec GetSpec(int accountId);
        List<Object[]> getAllAccounts();
    }
}

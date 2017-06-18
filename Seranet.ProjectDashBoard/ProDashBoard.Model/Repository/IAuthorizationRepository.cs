using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProDashBoard.Model.Repository
{
    public interface IAuthorizationRepository
    {
        string getUsername();
        bool getLoggedInUserAuthentication(int id);
        bool isAuthorized(int accountId);
    }
}

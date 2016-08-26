using ProDashBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProDashBoard.DAL.AuthorizationRepo
{
    interface IAuthorizationRepository
    {
        string getUsername();
        bool getLoggedInUserAuthentication(int id);
    }
}

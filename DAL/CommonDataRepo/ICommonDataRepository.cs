using ProDashBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProDashBoard.DAL.CommonDataRepo
{
    interface ICommonDataRepository
    {
       CommonData getSelectedProjectCommonData(int projectid);
    }
}

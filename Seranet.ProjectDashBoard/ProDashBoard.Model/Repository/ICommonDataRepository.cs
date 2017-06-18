using ProDashBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProDashBoard.Model.Repository
{
    public interface ICommonDataRepository
    {
       CommonData getSelectedProjectCommonData(int projectid);
        int update(CommonData commonData);
        int add(CommonData commonData);
    }
}

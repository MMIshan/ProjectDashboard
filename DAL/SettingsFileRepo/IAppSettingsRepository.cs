using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProDashBoard.DAL.SettingsFileRepo
{
    interface IAppSettingsRepository
    {
        string getThreshold();
        string getSpecLink();
        string getProcessComplianceLink();
    }
}

using ProDashBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProDashBoard.DAL.ProcessComplianceRepo
{
    interface IProcessComplianceQPRepository
    {
        List<ProcessComplianceQualityParameters> get();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProDashBoard.Model.Repository
{
    public interface ISpecRepository
    {
        int getSpecLevel(string projectIdToSpec);

        int getSpecProjectId(string projectCode);
    }
}

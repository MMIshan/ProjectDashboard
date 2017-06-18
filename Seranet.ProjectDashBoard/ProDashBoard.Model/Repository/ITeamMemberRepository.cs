using ProDashBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProDashBoard.Model.Repository
{
   public interface ITeamMemberRepository
    {

        List<TeamMembers> Get();

        List<TeamMembers> getActiveTeamMembers();
        TeamMembers Get(int id);
        TeamMembers getSelectedEmployee(string username);
        int add(TeamMembers teamMembers);
    }
}

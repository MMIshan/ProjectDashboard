
using ProDashBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProDashBoard.Model.Repository
{
    public interface IQuestionRepository
    {
        List<Questions> Get();

        List<Questions> getDisplayingQuestions(int availability);
        Questions Get(int id);

        
    }
}

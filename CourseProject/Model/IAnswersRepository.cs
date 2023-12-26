using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseProjectUserApplication.Model
{
    internal interface IAnswersRepository
    {
        List<AnswerModel> GetAnswersByRequestId(int id);
        void EditAnswer(int? id);
    }
}

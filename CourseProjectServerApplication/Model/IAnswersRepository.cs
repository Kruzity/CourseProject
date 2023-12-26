using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseProjectServerApplication.Model
{
    internal interface IAnswersRepository
    {
        void AddAnswerr(AnswerModel answer, int requestId);
    }
}

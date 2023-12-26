using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CourseProjectUserApplication.Model
{
    public interface IRequestRepository
    {
        void AddRequest(RequestModel request);
        void EditRequest(RequestModel request);
        List<RequestModel> GetByUserId(int id);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CourseProjectServerApplication.Model
{
    public interface IRequestRepository
    {
        void AddRequest(RequestModel request);
        void EditRequest(RequestModel request);
        void RemoveRequest(int id);
        List<RequestModel> GetByAll();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourseProjectServerApplication.ViewModel;

namespace CourseProjectServerApplication.Model
{
    public enum RequestState
    {
        Viewed,
        Unviewed,
        Processing,
        Done,
    }
    public enum ViewStates
    {
        All,
        Viewed,
        Unviewed,
        Processing,
        Done,
    }

    public class RequestModel : ViewModelBase
    {
        int _requestId;
        int _userId;
        string _title;
        string _message;
        RequestState _state;
        string _requestedDate;
        string _doneDate;

        public RequestModel(int requestId, int userId, string title, string message, RequestState state, string requestedDate, string doneDate)
        {
            RequestId = requestId;
            UserId = userId;
            Title = title;
            Message = message;
            State = state;
            RequestedDate = requestedDate;
            DoneDate = doneDate;
        }

        public int RequestId { get => _requestId; set => _requestId = value; }
        public int UserId { get => _userId; set => _userId = value; }
        public string Title { get => _title; set => _title = value; }
        public string Message { get => _message; set => _message = value; }
        public RequestState State { get => _state; set { _state = value; OnPropertyChanged(nameof(State)); } }
        public string RequestedDate { get => _requestedDate; set => _requestedDate = value; }
        public string DoneDate { get => _doneDate; set => _doneDate = value; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourseProjectServerApplication.ViewModel;

namespace CourseProjectServerApplication.Model
{
    internal class AnswerModel : ViewModelBase
    {
        string _message;
        string _date;
        bool _isReaded;

        public string Message { get => _message; set => _message = value; }
        public string Date { get => _date; set => _date = value; }
        public bool IsReaded { get => _isReaded; set { _isReaded = value; OnPropertyChanged(nameof(IsReaded)); } }

        public AnswerModel(string message, string date, bool isreaded)
        {
            Message = message;
            Date = date;
            IsReaded = isreaded;
        }

        public AnswerModel()
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourseProjectUserApplication.ViewModel;

namespace CourseProjectUserApplication.Model
{
    internal class AnswerModel : ViewModelBase
    {
        int id;
        string _message;
        string _date;
        bool _isReaded;

        public string Message { get => _message; set => _message = value; }
        public string Date { get => _date; set => _date = value; }
        public bool IsReaded { get => _isReaded; set { _isReaded = value; OnPropertyChanged(nameof(IsReaded)); } }
        public int Id { get => id; set => id = value; }

        public AnswerModel(int id,string message, string date, bool isreaded)
        {
            Id = id;
            Message = message;
            Date = date;
            IsReaded = isreaded;
        }

        public AnswerModel()
        {

        }
    }
}

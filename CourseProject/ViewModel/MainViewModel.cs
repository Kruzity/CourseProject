using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using CourseProjectUserApplication.Model;
using CourseProjectUserApplication.Repositories;

namespace CourseProjectUserApplication.ViewModel
{
    internal class MainViewModel : ViewModelBase
    {
        #region Fields

        DispatcherTimer timer = new DispatcherTimer();

        int _userId = 1;
        string _name = "";

        string _errorMessage = "";

        RequestModel _request = new RequestModel();
        RequestRepository rp = new RequestRepository();
        AnswerRepository ap = new AnswerRepository();

        List<RequestModel> _requests = new List<RequestModel>();

        ObservableCollection<RequestModel> _requestList = new ObservableCollection<RequestModel>();
        
        RequestModel _selectedItem;

        ObservableCollection<AnswerModel> _answersList = new ObservableCollection<AnswerModel>();

        #endregion

        #region Properties

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }
        public ObservableCollection<AnswerModel> AnswersList
        {
            get => _answersList;
            set
            {
                _answersList = value;
                OnPropertyChanged(nameof(AnswersList));
            }
        }
        public RequestModel SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }
        public ObservableCollection<RequestModel> RequestList
        {
            get => _requestList;
            set
            {
                _requestList = value;
                OnPropertyChanged(nameof(RequestList));
            }
        }
        public RequestModel NewRequest
        {
            get => _request;
            set
            {
                _request = value;
                OnPropertyChanged(nameof(NewRequest));
            }
        }

        public string Name
        {
            get => $"({_userId}) {_name}";
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public int UserId
        {
            get => _userId;
            set
            {
                _userId = value;
                OnPropertyChanged(nameof(UserId));
            }
        }

        #endregion

        #region Constructor

        public MainViewModel()
        {
            timer.Interval = TimeSpan.FromSeconds(10);
            //timer.Interval = TimeSpan.FromMinutes(2);
            timer.Tick += (o, e) =>
            {
                try
                {
                    ExecuteUpdateRequestsCommand(null);
                }
                catch (Exception)
                {
                    //MessageBox.Show(ex.Message);
                }
            };


            UserId = (int)App.Current.Properties["userId"];
            Name = App.Current.Properties["name"].ToString();
            

            OnClose = new ViewModelCommand(ExecuteOnCloseCommand);

            UpdateRequestsCommand = new ViewModelCommand(ExecuteUpdateRequestsCommand);
            NewRequestCommand = new ViewModelCommand(ExecuteNewRequestCommand, CanExecuteNewRequestCommand);
            GetAnswers = new ViewModelCommand(ExecuteGetAnswersCommand);
            GetRequests = new ViewModelCommand(ExecuteGetRequestsComand);

            MarkAsReadedAnswers = new ViewModelCommand(ExecuteMarkAsReadedAsnwersCommand);

            ExecuteUpdateRequestsCommand(null);
            
            timer.Start();
        }

        #endregion

        #region Methods

        private void ExecuteMarkAsReadedAsnwersCommand(object obj)
        {
            if (SelectedItem == null) return;

            if (string.IsNullOrWhiteSpace(SelectedItem.UnreadedMessages)) return;

            timer.Stop();

            foreach (var answer in AnswersList)
            {
                if (!answer.IsReaded)
                {
                    answer.IsReaded = true;
                    ap.EditAnswer(answer.Id);
                }
            }

            SelectedItem.UnreadedMessages = " ";

            timer.Start();
        }
        private void ExecuteOnCloseCommand(object obj)
        {
            timer.Stop();
        }
        private void ExecuteUpdateRequestsCommand(object obj)
        {
            if (_requests == null) return;

            ExecuteGetRequestsComand(null);
            RequestList = new ObservableCollection<RequestModel>(SortRequests(_requests));
        }
        private void ExecuteGetRequestsComand(object obj)
        {
            var new_requests = rp.GetByUserId(_userId);

            if (_requests.Count == 0) _requests = new_requests;
            else
            {
                for (int i = _requests.Count; i < new_requests.Count; i++) _requests.Add(new_requests[i]);

                for(int i = 0;i <_requests.Count; i++)
                {
                    if (_requests[i].State != new_requests[i].State || 
                        _requests[i].DoneDate != new_requests[i].DoneDate ||
                        _requests[i].UnreadedMessages != new_requests[i].UnreadedMessages) _requests[i] = new_requests[i];
                }
            }
        }

        private void ExecuteGetAnswersCommand(object obj)
        {
            if (SelectedItem == null) return;

            AnswersList = new ObservableCollection<AnswerModel>(ap.GetAnswersByRequestId(SelectedItem.RequestId));
        }

        private bool CanExecuteNewRequestCommand(object obj)
        {
            bool isValid = true;
            ErrorMessage = "";

            if (string.IsNullOrEmpty(_request.Title) || string.IsNullOrEmpty(_request.Message)) isValid = false;

            if (_request.Title.Length > 100) { ErrorMessage = "* Title too long, please make under 100 symbols *"; isValid = false; }
            if (_request.Message.Length > 300) { ErrorMessage = "* Message too long, please make under 300 symbols *"; isValid = false; }

            OnPropertyChanged(nameof(ErrorMessage));

            return isValid;
        }

        private void ExecuteNewRequestCommand(object obj)
        {

            NewRequest.UserId = UserId;
            NewRequest.State = RequestState.Unviewed;
            NewRequest.RequestedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            rp.AddRequest(NewRequest);

            NewRequest = new RequestModel();

        }

        public static List<RequestModel> SortRequests(List<RequestModel> requests)
        {
            // Разделение заявок на те, у которых есть не прочитанные сообщения, и остальные
            var unreadRequests = requests.Where(r => Convert.ToInt32(string.IsNullOrWhiteSpace(r.UnreadedMessages) ? "0" : r.UnreadedMessages) > 0).OrderBy(r => r.State).ToList();
            var readRequests = requests.Except(unreadRequests).OrderBy(r => r.State).ToList();

            // Объединение отсортированных заявок
            return unreadRequests.Concat(readRequests).ToList();
        }

        #endregion

        #region Commands

        public ICommand UpdateRequestsCommand { get; }
        public ICommand NewRequestCommand { get; }
        public ICommand GetAnswers { get; }
        public ICommand GetRequests { get; }
        public ICommand MarkAsReadedAnswers { get; }

        public ICommand OnClose { get; }

        #endregion
    }
}

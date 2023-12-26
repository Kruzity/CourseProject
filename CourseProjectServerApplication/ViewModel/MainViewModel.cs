using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using CourseProjectServerApplication.Model;
using CourseProjectServerApplication.Repositories;
using System.Timers;
using System.Windows.Threading;
using System.Windows.Controls;

namespace CourseProjectServerApplication.ViewModel
{
    internal class MainViewModel : ViewModelBase
    {
        #region Fields

        //Массиви дат для фільтрування запросів
        List<DateTime> requestedDates = new List<DateTime>();
        List<DateTime> doneDates = new List<DateTime>();

        //Таймер, який відповідаж за оновлення масиву запросів кожний n-ий відрізок часу
        DispatcherTimer timer = new DispatcherTimer();

        bool _isItemSelected = false;
        RequestModel _selectedItem = null;

        //Массив, який відповідає за відображення запросів користувачу
        ObservableCollection<RequestModel> requestList = new ObservableCollection<RequestModel>();

        //Массив, який зберігає усі запроси з бази данних
        List<RequestModel> Requests = new List<RequestModel>();

        //Відфільтрований массив запросів
        List<RequestModel> filteredRequests = new List<RequestModel>();

        //Репозіторії для роботи з базою данних
        RequestRepository rp = new RequestRepository();
        AnswerRepository ap = new AnswerRepository();

        string _searchableText = "";

        ViewStates _currentViewState = ViewStates.All;

        #endregion
        
        #region Parametres

        public ViewStates ViewState
        {
            get => _currentViewState;
            set
            {
                Enum.TryParse(value.ToString(), out _currentViewState);
                OnPropertyChanged(nameof(ViewState));
            }
        }
        public string SeachableText
        {
            get => _searchableText;
            set
            {
                _searchableText = value;

                OnPropertyChanged(nameof(SeachableText));

                ExecuteShowRequestsCommand(ViewState);
            }
        }
        //Параметр, який зберігає усі можливі стани запроса для роботи додатку
        public Array StateList
        {
            get
            {
                return Enum.GetValues(typeof(RequestState));
            }
        }
        
        //Параметр, який зберігає усі можливі стани відображення для роботи додатку
        public Array ViewStatesList
        {
            get
            {
                return Enum.GetValues(typeof(ViewStates));
            }
        }

        public bool IsItemSelected
        {
            get
            {
                return _isItemSelected;
            }
            set
            {
                _isItemSelected = value;

                if (_isItemSelected == false) ExecuteShowRequestsCommand(ViewState);

                OnPropertyChanged(nameof(IsItemSelected));
            }
        }
        public RequestModel SelectedItem 
        { 
            get
            {
                return _selectedItem;
            }
            set
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));

                if (_selectedItem != null) IsItemSelected = true;
                else IsItemSelected = false;
            }
        }
        public ObservableCollection<RequestModel> RequestList
        {
            get
            {
                return requestList;
            }
            set
            {
                requestList = value;
                OnPropertyChanged(nameof(RequestList));
            }
        }

        #endregion

        #region Contructor

        public MainViewModel()
        {
            //Налаштування таймеру для пришвидшення перевірки
            timer.Interval = TimeSpan.FromSeconds(10);

            //Налаштування таймеру для роботи
            //timer.Interval = TimeSpan.FromMinutes(2);
            timer.Tick += (o, e) =>
            {
                ExecuteGetRequestsCommand(null);
                ExecuteShowRequestsCommand(ViewState);
                //Console.WriteLine("\tUpdated by timer\n");
            };


            OnClose = new ViewModelCommand(ExecuteOnCloseCommand);

            GetRequests = new ViewModelCommand(ExecuteGetRequestsCommand, null);
            EditRequestCommand = new ViewModelCommand(ExecuteEditRequestCommand, null);
            ShowRequests = new ViewModelCommand(ExecuteShowRequestsCommand, null);

            MarkAsViewed = new ViewModelCommand(ExecuteMarkAsViewedCommand, CanExecuteMarkAsViewedCommand);
            SendAnswer = new ViewModelCommand(ExecuteSendAnswerCommand, CanExecuteSendAnswerCommand);

            RequestedDatesFilter = new ViewModelCommand(ExecuteRequestedDatesFilterCommand);
            DoneDatesFilter = new ViewModelCommand(ExecuteDoneDatesFilterCommand);
            
            ExecuteGetRequestsCommand(null);
            ExecuteShowRequestsCommand(ViewState);

            timer.Start();
        }

        #endregion

        #region  Methods

        private void ExecuteDoneDatesFilterCommand(object obj)
        {
            (doneDates = ((SelectedDatesCollection)obj).ToList()).ForEach((e) => e = e.Date);

            ExecuteShowRequestsCommand(ViewState);
        }

        private void ExecuteRequestedDatesFilterCommand(object obj)
        {
            (requestedDates = ((SelectedDatesCollection)obj).ToList()).ForEach((e) => e = e.Date);

            ExecuteShowRequestsCommand(ViewState);
        }
        private bool CanExecuteSendAnswerCommand(object obj)
        {
            return !string.IsNullOrEmpty((string)obj);
        }
        private void ExecuteSendAnswerCommand(object obj)
        {
            //MessageBox.Show("added");
            ap.AddAnswerr(new AnswerModel(obj.ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), false), SelectedItem.RequestId);

            SelectedItem = null;
        }
        private void ExecuteOnCloseCommand(object obj)
        {
            timer.Stop();
        }
        private bool CanExecuteMarkAsViewedCommand(object obj)
        {
            if (SelectedItem == null) return false;

            return SelectedItem.State == RequestState.Unviewed;
        }
        private void ExecuteMarkAsViewedCommand(object obj)
        {
            SelectedItem.State = RequestState.Viewed;
        }
        
        //Метод, який відповідає за зміну відображаємих запросів та їх фільтрацію
        private void ExecuteShowRequestsCommand(object obj)
        {
            filteredRequests = new List<RequestModel>(Requests);

            //Корректна фільтрація по датам



            foreach (var req in filteredRequests.ToList())
            {
                //Console.WriteLine(req.RequestId.ToString() + " - " + req.RequestedDate + " - " + req.DoneDate);
                if (requestedDates.Count > 0 && doneDates.Count > 0)
                {
                    if (!requestedDates.Contains(DateTime.Parse(req.RequestedDate).Date) &&
                        !doneDates.Contains(req.DoneDate == "" ? DateTime.MinValue.Date : DateTime.Parse(req.DoneDate).Date)) filteredRequests.Remove(req);
                }
                else if (requestedDates.Count > 0 && doneDates.Count == 0)
                {
                    if (!requestedDates.Contains(DateTime.Parse(req.RequestedDate).Date))
                        filteredRequests.Remove(req);
                }
                else if (requestedDates.Count == 0 && doneDates.Count > 0)
                {
                    if (!doneDates.Contains(req.DoneDate == "" ? DateTime.MinValue.Date : DateTime.Parse(req.DoneDate).Date)) filteredRequests.Remove(req);
                }
            }

            //Console.WriteLine("\n---------------------------------");

            //Фільтрацію по тексту
            if (!string.IsNullOrWhiteSpace(SeachableText) || !string.IsNullOrEmpty(SeachableText))
            {
                foreach(var req in filteredRequests.ToList())
                {
                    if (!req.Title.ToLower().Contains(SeachableText.ToLower()) && 
                        !req.Message.ToLower().Contains(SeachableText.ToLower()) &&
                        !req.UserId.ToString().ToLower().Contains(SeachableText.ToLower()) &&
                        !req.RequestId.ToString().ToLower().Contains(SeachableText.ToLower())) filteredRequests.Remove(req);
                }

            }

            //Фільтрація по стану відображення
            switch (ViewState)
            {
                case (ViewStates.All):
                    {
                        RequestList = new ObservableCollection<RequestModel>(filteredRequests);
                        break;
                    }
                case (ViewStates.Viewed):
                    {
                        RequestList = new ObservableCollection<RequestModel>(filteredRequests.Where(x => x.State == RequestState.Viewed).ToList());
                        break;
                    }
                case (ViewStates.Unviewed):
                    {
                        RequestList = new ObservableCollection<RequestModel>(filteredRequests.Where(x => x.State == RequestState.Unviewed).ToList());
                        break;
                    }
                case (ViewStates.Processing):
                    {
                        RequestList = new ObservableCollection<RequestModel>(filteredRequests.Where(x => x.State == RequestState.Processing).ToList());
                        break;
                    }
                case (ViewStates.Done):
                    {
                        RequestList = new ObservableCollection<RequestModel>(filteredRequests.Where(x => x.State == RequestState.Done).ToList());
                        break;
                    }
            }
        }

        //Метод який відповідає за зміну данних реквесту, таких як Статус та Дата завершення
        private void ExecuteEditRequestCommand(object obj)
        {
            if (obj == null) return;

            if (SelectedItem.State != (RequestState)obj)
            {
                SelectedItem.State = (RequestState)obj;

                if (SelectedItem.State == RequestState.Done) SelectedItem.DoneDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                else SelectedItem.DoneDate = "";
            }
           

            rp.EditRequest(SelectedItem);
        }

        private void ExecuteGetRequestsCommand(object obj)
        {
             var new_requests = rp.GetByAll();

             Requests.AddRange(new_requests.Where(r2 => !Requests.Any(r1 => r1.RequestId == r2.RequestId)));
        }

        #endregion

        #region Commands

        public ICommand OnClose { get; }

        public ICommand GetRequests { get; }
        public ICommand ShowRequests { get; }
        public ICommand EditRequestCommand { get; }
        public ICommand MarkAsViewed { get; }
        public ICommand SendAnswer { get; }
        public ICommand RequestedDatesFilter { get; }
        public ICommand DoneDatesFilter { get; }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CourseProjectServerApplication.Repositories;
using CourseProjectServerApplication.Model;
using System.Net;
using System.Windows;
using System.Threading;
using System.Security.Principal;

namespace CourseProjectServerApplication.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        #region Fields
        string _login;
        string _password;
        int _userid;
        string _errorMessage;

        //Поле яке відпопідає за відображення вікна логування, воно змінюється коли користувачу
        //вдається здійснити логування
        bool _isViewVisible;

        AdminRepository ar = new AdminRepository();
        #endregion

        #region Properties
        public bool IsViewVisible
        {
            get
            {
                return _isViewVisible;
            }
            set
            {
                _isViewVisible = value;
                OnPropertyChanged(nameof(IsViewVisible));
            }
        }
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }
        public string Login 
        { 
            get => _login; 
            set
            {
                _login = value;
                OnPropertyChanged(nameof(Login));
            }
        }
        public string Password 
        { 
            get => _password; 
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        public int Userid { get => _userid; set => _userid = value; }
        #endregion

        #region Contructor
        public LoginViewModel()
        {
            LoginCommand = new ViewModelCommand(ExecuteLoginCommand, CanExecuteLoginCommand);
        }
        #endregion

        #region Methods
        private void ExecuteLoginCommand(object obj)
        {
            _userid = ar.AuthenticateAdmin(new NetworkCredential(Login, Password));

            if (_userid != -1)
            {
                ErrorMessage = "";
                MessageBox.Show($"Login complete Mr.{Login}!");
                IsViewVisible = false;
            }
            else ErrorMessage = "* Invalid username or password *";
        }

        //Перевірка на валідність данних у полях логін та пароль
        private bool CanExecuteLoginCommand(object obj)
        {
            bool validData;
            
            if (string.IsNullOrEmpty(Login) || string.IsNullOrEmpty(Password)) validData = false;
            else validData = true;

            return validData;
        }
        #endregion

        #region Commands
        public ICommand LoginCommand { get; }
        #endregion

    }
}

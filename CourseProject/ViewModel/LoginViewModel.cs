using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CourseProjectUserApplication.Repositories;
using CourseProjectUserApplication.Model;
using System.Net;
using System.Windows;
using System.Threading;
using System.Security.Principal;
using CourseProjectUserApplication;

namespace CourseProjectUserApplication.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        #region Fields

        string _login;
        string _password;
        string _errorMessage;

        bool _isViewVisible;

        UserRepository ur = new UserRepository();

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

        #endregion

        #region Constructor

        public LoginViewModel()
        {
            LoginCommand = new ViewModelCommand(ExecuteLoginCommand, CanExecuteLoginCommand);
        }

        #endregion

        #region Methods

        private void ExecuteLoginCommand(object obj)
        {
            int userId = ur.AuthenticateUser(new NetworkCredential(Login, Password));


            if (userId != -1)
            {
                App.Current.Properties.Add("userId", userId);
                App.Current.Properties.Add("name", Login);

                ErrorMessage = "";
                MessageBox.Show($"Login complete Mr.{Login}!");
                IsViewVisible = false;
            }
            else ErrorMessage = "* Invalid username or password *";
        }

        private bool CanExecuteLoginCommand(object obj)
        {
            return !(string.IsNullOrEmpty(Login) || string.IsNullOrEmpty(Password));
        }

        #endregion

        #region Commands
        public ICommand LoginCommand { get; }

        #endregion
    }
}

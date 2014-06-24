using System;
using System.Security.Cryptography;
using Caliburn.Micro;

namespace Employees.ViewModels
{
    public interface ILoginViewModel : IScreen
    {
        event EventHandler UserLogined;
        event EventHandler UserExit;
    }

    public class LoginViewModel : Screen, ILoginViewModel
    {
        public event EventHandler UserLogined;
        public event EventHandler UserExit;

        private string _userName;
        private string _password;

        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                NotifyOfPropertyChange(() => UserName);
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                NotifyOfPropertyChange(() => Password);
            }
        }


        public void Login()
        {
            RaiseUserLogined();

            Password = string.Empty;
        }

        public void Exit()
        {
            RaiseUserExit();
        }


        private void RaiseUserLogined()
        {
            if (UserLogined != null)
                UserLogined(null, null);
        }

        private void RaiseUserExit()
        {
            if (UserExit != null)
                UserExit(null, null);
        }
    }
}
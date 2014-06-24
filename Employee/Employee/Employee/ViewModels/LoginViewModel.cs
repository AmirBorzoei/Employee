using Caliburn.Micro;
using Employees.DAL.Repositories;

namespace Employees.ViewModels
{
    public delegate void UserLoginedEventHandler(bool userChanged);

    public delegate void UserExitEventHandler();

    public interface ILoginViewModel : IScreen
    {
        event UserLoginedEventHandler UserLogined;
        event UserExitEventHandler UserExit;
    }

    public class LoginViewModel : Screen, ILoginViewModel
    {
        private readonly UserRepository _userRepository;
        private string _password;
        private string _userName;


        public LoginViewModel(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }


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


        public event UserLoginedEventHandler UserLogined;
        public event UserExitEventHandler UserExit;


        public void Login()
        {
            var user = _userRepository.ValidateUser(UserName, Password);
            if (user != null)
            {
                RaiseUserLogined(true);

                Password = string.Empty;
            }
        }

        public void Exit()
        {
            RaiseUserExit();
        }


        private void RaiseUserLogined(bool userChanged)
        {
            if (UserLogined != null)
                UserLogined(userChanged);
        }

        private void RaiseUserExit()
        {
            if (UserExit != null)
                UserExit();
        }
    }
}
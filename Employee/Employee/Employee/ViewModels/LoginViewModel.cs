using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Caliburn.Micro;
using Employees.DAL.Repositories;
using Employees.Shared.Permission;
using Employees.Shared.Services;

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
        private Visibility _errorMaessageVisibility;


        public LoginViewModel(UserRepository userRepository)
        {
            _userRepository = userRepository;
            _errorMaessageVisibility = Visibility.Hidden;
        }


        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                NotifyOfPropertyChange(() => UserName);

                if (ErrorMaessageVisibility == Visibility.Visible)
                    ErrorMaessageVisibility = Visibility.Hidden;
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                NotifyOfPropertyChange(() => Password);

                if (ErrorMaessageVisibility == Visibility.Visible)
                    ErrorMaessageVisibility = Visibility.Hidden;
            }
        }

        public Visibility ErrorMaessageVisibility
        {
            get { return _errorMaessageVisibility; }
            set
            {
                _errorMaessageVisibility = value;
                NotifyOfPropertyChange(() => ErrorMaessageVisibility);
            }
        }


        public event UserLoginedEventHandler UserLogined;
        public event UserExitEventHandler UserExit;


        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

#if Develop
            Login(true);
#endif
        }


        public void PasswordKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                Login();
            }
            else if (e.Key == Key.Escape)
            {
                Exit();
            }
        }

        public void Login(bool isFirstTime = false)
        {
            if (ErrorMaessageVisibility == Visibility.Visible)
                ErrorMaessageVisibility = Visibility.Hidden;


            Task.Factory.StartNew(ProgressBarService.Show);

            var task = Task.Factory.StartNew(() =>
            {
                var loginedUser = _userRepository.ValidateUser(UserName, Password, isFirstTime);
                if (loginedUser == null)
                {
                    ErrorMaessageVisibility = Visibility.Visible;
                }
                else
                {
                    Sission.LoginedUser = loginedUser;
                    if (Application.Current.Resources.Contains(App.LoginedUserResourceKey))
                    {
                        Application.Current.Resources[App.LoginedUserResourceKey] = Sission.LoginedUser;
                    }

                    var userChanged = Sission.LoginedUser == null || Sission.LoginedUser.User.UserName != UserName;
                    RaiseUserLogined(userChanged);

                    Password = string.Empty;
                }
            });

            task.ContinueWith(arg => Task.Factory.StartNew(ProgressBarService.Hide));
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
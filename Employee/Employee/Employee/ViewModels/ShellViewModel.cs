using System.Windows;
using Caliburn.Micro;
using DevExpress.Xpf.Core;
using Employees.Administration.ViewModels;
using Employees.Personally.ViewModels;
using Employees.Shared.Events;
using Employees.Shared.Interfaces;
using Employees.Shared.ViewModels;

namespace Employees.ViewModels
{
    public interface IShellViewModel
    {
        void Start();
        void Stop();
    }

    public class ShellViewModel : WorkspaceBase, IShellViewModel
    {
        private readonly ILoginViewModel _loginViewModel;
        private readonly IAdministrationWorkspaceViewModel _administrationWorkspaceViewModel;
        private readonly IEmployeeInfoViewModel _employeeInfoViewModel;
        private Visibility _loginViewVisibility;
        private Visibility _progressbarVisibility;


        public ShellViewModel(IEventAggregator eventAggregator,
                              ILoginViewModel loginViewModel,
                              IAdministrationWorkspaceViewModel administrationWorkspaceViewModel,
                              IEmployeeInfoViewModel employeeInfoViewModel) : base(eventAggregator)
        {
            _loginViewModel = loginViewModel;
            _loginViewModel.UserLogined += UserLogined;
            _loginViewModel.UserExit += UserExit;

            _administrationWorkspaceViewModel = administrationWorkspaceViewModel;
            _employeeInfoViewModel = employeeInfoViewModel;

            DisplayName = "* * *";
            _loginViewVisibility = Visibility.Visible;
            _progressbarVisibility = Visibility.Collapsed;
        }


        public ILoginViewModel LoginViewModel
        {
            get { return _loginViewModel; }
        }

        public Visibility LoginViewVisibility
        {
            get { return _loginViewVisibility; }
            private set
            {
                _loginViewVisibility = value;
                NotifyOfPropertyChange(() => LoginViewVisibility);
            }
        }

        public Visibility ProgressbarVisibility
        {
            get { return _progressbarVisibility; }
            private set
            {
                _progressbarVisibility = value;
                NotifyOfPropertyChange(() => ProgressbarVisibility);
            }
        }

        #region Menu Handler

        public void ShowBasicData()
        {
            ActivateItem(null);
        }

        public void ShowFinancial()
        {
            ActiveItem = null;
        }

        public void ShowPersonally()
        {
            ActivateItem(_employeeInfoViewModel);
        }

        public void ShowAdministration()
        {
            ActivateItem(_administrationWorkspaceViewModel);
        }

        #endregion Menu Handler

        #region Toolbar Handler

        public void Save()
        {
            var item = ActiveItem as ISupportSave;
            if (item != null)
                item.Save();
        }

        public void Reload()
        {
            var item = ActiveItem as ISupportReload;
            if (item != null)
                item.Reload();
        }

        public void New()
        {
            var item = ActiveItem as ISupportNew;
            if (item != null)
                item.New();
        }

        public void Print()
        {
            var item = ActiveItem as ISupportPrint;
            if (item != null)
                item.Print();
        }

        public void Logout()
        {
            LoginViewVisibility = Visibility.Visible;
        }

        public void ChangeTheme()
        {
            switch (ThemeManager.ApplicationThemeName)
            {
                case "DXStyle":
                    ThemeManager.ApplicationThemeName = "DeepBlue";
                    break;
                case "DeepBlue":
                    ThemeManager.ApplicationThemeName = "Office2013";
                    break;
                default:
                    ThemeManager.ApplicationThemeName = "DXStyle";
                    break;
            }
        }

        #endregion Toolbar Handler

        #region Login Handler

        private void UserLogined(bool changedUser)
        {
            LoginViewVisibility = Visibility.Collapsed;
        }

        private void UserExit()
        {
            TryClose();
        }

        #endregion Login Handler

        #region ProgressBar Handler

        public void Start()
        {
            ProgressbarVisibility = Visibility.Visible;
        }

        public void Stop()
        {
            ProgressbarVisibility = Visibility.Collapsed;
        }

        #endregion ProgressBar Handler
    }
}
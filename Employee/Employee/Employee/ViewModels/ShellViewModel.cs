using System.Windows;
using Caliburn.Micro;
using DevExpress.Xpf.Core;
using Employees.Administration.ViewModels;
using Employees.Personally.ViewModels;
using Employees.Shared.Interfaces;
using Employees.Shared.Models;
using Employees.Shared.Permission;
using Employees.Shared.Services;
using Employees.Shared.ViewModels;

namespace Employees.ViewModels
{
    public interface IShellViewModel
    {
    }
    public class ShellViewModel : WorkspaceBase, IShellViewModel, IProgressBarService
    {
        private readonly ILoginViewModel _loginViewModel;

        private IAdministrationWorkspaceViewModel _administrationWorkspaceViewModel;
        private IEmployeeInfoViewModel _employeeInfoViewModel;

        private Visibility _loginViewVisibility;
        private Visibility _progressbarVisibility;
        private LoginedUser _loginedUser;


        public ShellViewModel(IEventAggregator eventAggregator,
            ILoginViewModel loginViewModel) : base(eventAggregator)
        {
            _loginViewModel = loginViewModel;
            _loginViewModel.UserLogined += UserLogined;
            _loginViewModel.UserExit += UserExit;

            DisplayName = "اطلاعات کارمندان";
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

        public LoginedUser LoginedUser
        {
            get { return _loginedUser; }
            set
            {
                _loginedUser = value;
                NotifyOfPropertyChange(() => LoginedUser);
            }
        }

        #region Menu Handler

        public void ShowAdministration()
        {
            if (_administrationWorkspaceViewModel == null)
                _administrationWorkspaceViewModel = IoC.Get<IAdministrationWorkspaceViewModel>();

            ActivateItem(_administrationWorkspaceViewModel);
        }

        public void ShowBasicData()
        {
            ActiveItem = null;
        }

        public void ShowFinancial()
        {
            ActiveItem = null;
        }

        public void ShowPersonally()
        {
            if (_employeeInfoViewModel == null)
                _employeeInfoViewModel = IoC.Get<IEmployeeInfoViewModel>();

            ActivateItem(_employeeInfoViewModel);
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
                item.Print(GetView() as Window);
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

            Items.Clear();
            ActiveItem = null;
            _administrationWorkspaceViewModel = null;
            _employeeInfoViewModel = null;

            LoginedUser = Sission.LoginedUser;
        }

        private void UserExit()
        {
            TryClose();
        }

        #endregion Login Handler

        #region ProgressBar Handler

        public void ProgressBarShow()
        {
            ProgressbarVisibility = Visibility.Visible;
        }

        public void ProgressBarHide()
        {
            ProgressbarVisibility = Visibility.Collapsed;
        }

        #endregion ProgressBar Handler
    }
}
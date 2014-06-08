using System;
using Caliburn.Micro;
using DevExpress.Xpf.Docking;
using Employees.Shared.Models;

namespace Employees.Shared.ViewModels
{
    public interface IScreenBase : IScreen, IMVVMDockingProperties
    {
    }


    public class ScreenBase<TModel> : Screen, IScreenBase
        where TModel : BaseModel
    {
        private TModel _currentObject;


        public virtual TModel CurrentObject
        {
            get { return _currentObject; }
            set
            {
                _currentObject = value;
                NotifyOfPropertyChange(() => CurrentObject);
                CurrentObjectChanged();
            }
        }

        public string TargetName
        {
            get { return "documentContainer"; }
            set { throw new NotImplementedException(); }
        }


        protected virtual void CurrentObjectChanged()
        {
            DisplayName = _currentObject == null ? string.Empty : _currentObject.DisplayName;
        }
    }
}
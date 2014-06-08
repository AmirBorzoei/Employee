using Caliburn.Micro;
using Employees.Shared.Constants;

namespace Employees.Shared.Models
{
    public class BaseModel : PropertyChangedBase
    {
        public ModelStates State { get; set; }

        public bool IsDirty
        {
            get { return State != ModelStates.Unchanged; }
        }

        public virtual string DisplayName
        {
            get { return GetType().ToString(); }
        }


        public BaseModel()
        {
            State = ModelStates.New;

            PropertyChanged += BaseModel_PropertyChanged;
        }


        public override string ToString()
        {
            return DisplayName;
        }


        private void BaseModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (State == ModelStates.Unchanged && e.PropertyName != "State")
            {
                State = ModelStates.Modified;
                NotifyOfPropertyChange(() => IsDirty);
            }
        }
    }
}
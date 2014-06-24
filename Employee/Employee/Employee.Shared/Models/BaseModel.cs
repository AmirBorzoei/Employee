using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using Employees.Shared.Attributes;
using Employees.Shared.Constants;

namespace Employees.Shared.Models
{
    public class BaseModel : PropertyChangedBase
    {
        public BaseModel()
        {
            BeginInit();

            State = ModelStates.New;

            EndInit();
        }


        [IgnoreChangeState]
        public ModelStates State { get; set; }

        public virtual bool IsEmpty
        {
            get { return false; }
        }

        public bool IsDirty
        {
            get
            {
                if (IsEmpty) return false;

                if (State == ModelStates.New || State == ModelStates.Modified || State == ModelStates.Deleted)
                {
                    return true;
                }

                var infos = GetType().GetProperties();

                if (infos.Length == 0) return false;

                var propertyInfos = infos.Where(info => Attribute.IsDefined(info, typeof (ConsiderIsDirtyAttribute))).ToList();

                if (propertyInfos.Count == 0) return false;

                foreach (var info in propertyInfos)
                {
                    if (info.PropertyType.BaseType == typeof (BaseModel))
                    {
                        var value = info.GetValue(this, null) as BaseModel;
                        if (value != null && value.IsDirty) return true;
                    }

                    var values = info.GetValue(this, null) as IEnumerable<BaseModel>;
                    if (values == null) continue;
                    if (values.Any(baseModel => baseModel.IsDirty)) return true;

                    var infoDeletedItems = values.GetType().GetProperties().FirstOrDefault(x => x.Name == "DeletedItems");
                    if (infoDeletedItems == null) continue;
                    var valuesDeletedItems = infoDeletedItems.GetValue(values, null) as IEnumerable<BaseModel>;
                    if (valuesDeletedItems != null && valuesDeletedItems.Any(baseModel => baseModel.IsDirty)) return true;
                }

                return false;
            }
        }

        public virtual string DisplayName
        {
            get { return GetType().ToString(); }
        }


        public override void NotifyOfPropertyChange(string propertyName)
        {
            if (!IsNotifying)
                return;

            var info = GetType().GetProperty(propertyName);
            var ignore = Attribute.IsDefined(info, typeof (IgnoreChangeStateAttribute));

            if (!ignore && State == ModelStates.Unchanged) //Set the state to modified, if it is not added or deleted
                State = ModelStates.Modified;

            base.NotifyOfPropertyChange(propertyName);


            if (State == ModelStates.Unchanged && propertyName != "State")
            {
                State = ModelStates.Modified;
                NotifyOfPropertyChange(() => IsDirty);
            }
        }

        public override string ToString()
        {
            return DisplayName;
        }


        public void BeginInit()
        {
            IsNotifying = false;
        }

        public void EndInit()
        {
            IsNotifying = true;
        }
    }
}
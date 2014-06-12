using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.PresentationFramework;
using DrArchiv.Shared.Attributes;

namespace DrArchiv.Shared.Models
{
    public class BaseModel : PropertyChangedBase
    {
        public BaseModel()
        {
            BeginInit();

            State = ObjectStates.Added;

            EndInit();
        }

        [IgnoreChangeState]
        public virtual bool IsEmpty
        {
            get { return false; }
        }

        [IgnoreChangeState]
        public ObjectStates State { get; set; }

        [IgnoreChangeState]
        public bool IsDirty
        {
            get
            {
                if (IsEmpty) return false;

                if (State == ObjectStates.Added || State == ObjectStates.Updated || State == ObjectStates.Deleted)
                {
                    return true;
                }

                var infos = GetType().GetProperties();

                if (infos == null || infos.Length == 0) return false;

                var propertyInfos = infos.Where(info => Attribute.IsDefined(info, typeof (ConsiderIsDirtyAttribute))).ToList();

                if (propertyInfos == null || propertyInfos.Count == 0) return false;

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


        public override void NotifyOfPropertyChange(string propertyName)
        {
            if (!IsNotifying)
                return;

            var info = GetType().GetProperty(propertyName);
            var ignore = Attribute.IsDefined(info, typeof (IgnoreChangeStateAttribute));

            if (!ignore && State == ObjectStates.None) //Set the state to modified, if it is not added or deleted
                State = ObjectStates.Updated;

            base.NotifyOfPropertyChange(propertyName);
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
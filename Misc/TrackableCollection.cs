using System.Collections.Generic;
using Caliburn.PresentationFramework;

namespace DrArchiv.Shared.Models
{
    public class TrackableCollection<T> : BindableCollection<T> where T : BaseModel
    {
        public TrackableCollection()
        {
            DeletedItems = new List<T>();
        }

        public TrackableCollection(IEnumerable<T> collection)
            : base(CheckCollection(collection))
        {
            DeletedItems = new List<T>();
        }

        private static IEnumerable<T> CheckCollection(IEnumerable<T> collection)
        {
            if (collection == null)
                return new List<T>();

            return collection;
        }

        public virtual IList<T> DeletedItems
        {
            get;
            private set;
        }

        protected override void ClearItemsBase()
        {
            base.ClearItemsBase();
            DeletedItems.Clear();
        }

        protected override void RemoveItemBase(int index)
        {
            var item = this[index];

            base.RemoveItemBase(index);

            if (item.State != ObjectStates.Added)
            {
                item.State = ObjectStates.Deleted;
                DeletedItems.Add(item);
            }
        }

        public bool RemovePermanent(T item)
        {
            var index = IndexOf(item);
            if (index < 0)
                return false;

            base.RemoveItemBase(index);

            return true;
        }
    }
}
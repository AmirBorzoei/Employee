using Caliburn.Micro;

namespace Employees.Shared.Events
{
    public class ActiveItemChangedMessage
    {
        public ActiveItemChangedMessage(IScreen parentItem, IScreen activeItem)
        {
            ParentItem = parentItem;
            ActiveItem = activeItem;
        }


        public IScreen ParentItem { get; private set; }

        public IScreen ActiveItem { get; private set; }
    }
}
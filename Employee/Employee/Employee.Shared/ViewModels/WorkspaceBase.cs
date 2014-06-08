using System;
using System.Collections.Specialized;
using Caliburn.Micro;
using DevExpress.Xpf.Docking;
using DevExpress.Xpf.Docking.Base;
using Employees.Shared.Events;
using Employees.Shared.Models;
using Employees.Shared.View;

namespace Employees.Shared.ViewModels
{
    public interface IWorkspaceBase : IScreenBase
    {
    }

    public class WorkspaceBase : Conductor<IScreen>.Collection.OneActive, IWorkspaceBase
    {
        protected readonly IEventAggregator EventAggregator;
        protected IWorkspaceBaseView WorkspaceBaseView;

        public WorkspaceBase(IEventAggregator eventAggregator)
        {
            EventAggregator = eventAggregator;
        }


        public virtual BaseModel CurrentObject { get; set; }

        public string TargetName
        {
            get { return "documentContainer"; }
            set { throw new NotImplementedException(); }
        }


        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            WorkspaceBaseView = view as IWorkspaceBaseView;
        }

        protected override void ChangeActiveItem(IScreen newItem, bool closePrevious)
        {
            base.ChangeActiveItem(newItem, closePrevious);

            PublishActiveItemChangedMessage(newItem);
        }

        public override void ActivateItem(IScreen item)
        {
            base.ActivateItem(item);

            if (WorkspaceBaseView != null)
                WorkspaceBaseView.ChangedActiveItem();
        }


        public virtual void SelectedItemChanged(DockItemActivatedEventArgs e)
        {
            if (e == null || !IsActive) return;

            if (e.Item is DocumentPanel)
            {
                var panel = (DocumentPanel) e.Item;
                ChangeActiveItem(panel.Content as IScreen, false);
            }
            //else
            //{
            //    ChangeActiveItem(null, false);
            //}
        }

        public virtual void SelectedItemClosing(ItemCancelEventArgs e)
        {
            if (e == null) return;

            if (e.Item is DocumentPanel)
            {
                var panel = (DocumentPanel) e.Item;
                if (panel.Content is IScreen)
                {
                    var screen = (IScreen) panel.Content;
                    screen.TryClose();
                }
            }
        }


        protected virtual void PublishActiveItemChangedMessage(IScreen newItem)
        {
            if (IsActive)
                EventAggregator.Publish(new ActiveItemChangedMessage(this, newItem));
        }
    }
}
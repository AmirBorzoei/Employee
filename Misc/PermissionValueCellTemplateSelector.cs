using System.Windows;
using System.Windows.Controls;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DrArchiv.Proxy.Contracts;
using DrArchiv.Shared.Models;

namespace DrArchiv.Shared.Helper
{
    public class PermissionValueCellTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (System.ComponentModel.DesignerProperties.IsInDesignTool)
                return null;

            var rowData = ((GridCellData)item).RowData;
            var presenter = (FrameworkElement)container;
            var itemValue = rowData.Row as UserRolePermissionKey;

            if (itemValue == null) return null;

            switch (itemValue.PermissionKey.ValueType)
            {
                case permissionValueTypeEnum.Type_Boolean:
                    return (DataTemplate)presenter.FindResource("editorBool");

                case permissionValueTypeEnum.Type_Integer:
                    return (DataTemplate)presenter.FindResource("editorInt");

                case permissionValueTypeEnum.Type_Text:
                    return (DataTemplate)presenter.FindResource("editorString");

                case permissionValueTypeEnum.Type_Non:
                    return (DataTemplate)presenter.FindResource("editorNone");

                default:
                    return null;
            }
        }
    }
}
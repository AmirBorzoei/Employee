using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using DrArchiv.Shared.Constant;
using DrArchiv.Shared.Models;

namespace DrArchiv.Shared.Converters
{
    public class UserPermissionConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var user = value as User;
            var parameters = parameter.ToString().Split(';');
            var permissionKey = parameters[0];
            bool visibility;
            if (parameters.Length == 2)
                visibility = parameters[1] == "Visibility";
            else
                visibility = targetType == typeof (Visibility);

            if (permissionKey == "HealthRecordModule_PatientNameInOverviewList")
            {
                
            }
                

            if (user == null || user.PermissionKeys == null)
            {
                return visibility ? (object) Visibility.Collapsed : false;
            }

            if (user.HasAccess(permissionKey, AccessLevels.Hide))
            {
                return visibility ? (object) Visibility.Collapsed : false; //The boolean value not needed in system, it can be true!!!
            }

            if (user.HasAccess(permissionKey, AccessLevels.ReadOnly))
            {
                return visibility ? (object) Visibility.Visible : true;
            }

            if (user.HasAccess(permissionKey, AccessLevels.Active))
            {
                return visibility ? (object) Visibility.Visible : false;
            }

            return visibility ? (object) Visibility.Collapsed : false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
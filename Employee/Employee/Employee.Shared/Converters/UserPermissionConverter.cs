using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Employees.Shared.Constants;
using Employees.Shared.Models;

namespace Employees.Shared.Converters
{
    public class UserPermissionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var loginedUser = value as LoginedUser;
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

            if (loginedUser == null)
            {
                return visibility ? (object) Visibility.Collapsed : true;
            }
            if (loginedUser.HasAccess(permissionKey, PermissionAccessTypes.Hide))
            {
                return visibility ? (object) Visibility.Collapsed : true; //The boolean value not needed in system, it can be true!!!
            }
            if (loginedUser.HasAccess(permissionKey, PermissionAccessTypes.Readonly))
            {
                return visibility ? (object) Visibility.Visible : true;
            }
            if (loginedUser.HasAccess(permissionKey, PermissionAccessTypes.Active))
            {
                return visibility ? (object) Visibility.Visible : false;
            }

            return visibility ? (object) Visibility.Collapsed : true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
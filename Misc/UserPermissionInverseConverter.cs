using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using DrArchiv.Shared.Constant;
using DrArchiv.Shared.Models;

namespace DrArchiv.Shared.Converters
{
    public class UserPermissionInverseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var user = value as User;

            var visibility = targetType == typeof (Visibility);

            if (user == null || user.PermissionKeys == null)
            {
                return visibility ? (object) Visibility.Visible : true;
            }

            if (user.HasAccess(parameter.ToString(), AccessLevels.Hide))
            {
                return visibility ? (object) Visibility.Visible : false; //The boolean value not needed in system, it can be true!!! It use for DOBControl controls.
            }

            if (user.HasAccess(parameter.ToString(), AccessLevels.ReadOnly))
            {
                return visibility ? (object) Visibility.Collapsed : false;
            }

            if (user.HasAccess(parameter.ToString(), AccessLevels.Active))
            {
                return visibility ? (object) Visibility.Collapsed : true;
            }

            return visibility ? (object) Visibility.Visible : true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
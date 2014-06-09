using System;
using System.Linq;
using System.Windows.Data;
using System.Globalization;
using Employees.Shared.Models;
using System.Collections.Generic;

namespace Employees.Shared.Converters
{
    public class ObjectListToUserGroupListConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var objList = value as IEnumerable<object>;
            if (objList == null) return value;

            var userGroups = objList.Cast<UserGroup>().ToList();
            return userGroups;
        }
    }
}
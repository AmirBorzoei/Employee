using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Documents;
using Employees.Shared.Models;

namespace Employees.Shared.Converters
{
    public class TestConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            List<object> ol = new List<object>();

            if (value is IEnumerable<BaseModel>)
            {
                var l = (value as IEnumerable<BaseModel>);
                foreach (var v in l)
                {
                    ol.Add(v);
                }
            }

            return ol;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            List<BaseModel> ol = new List<BaseModel>();

            if (value is IEnumerable<object>)
            {
                var l = (value as IEnumerable<object>);
                foreach (var v in l)
                {
                    ol.Add(v as BaseModel);
                }
            }

            return ol;
        }
    }
}
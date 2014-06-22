using System;
using System.Windows.Data;
using System.Globalization;
using System.ComponentModel;
using DevExpress.Xpf.Core;

namespace Employees.Shared.Converters
{
    public class EnumDescriptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;

            Enum myEnum;
            var enumItem = value as EnumHelper.EnumItem;
            if (enumItem != null)
            {
                myEnum = (Enum) enumItem.Id;
            }
            else
            {
                myEnum = value as Enum;
            }

            if (myEnum == null) return value;

            var description = GetEnumDescription(myEnum);
            return description;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.Empty;
        }


        private string GetEnumDescription(Enum enumObj)
        {
            var fieldInfo = enumObj.GetType().GetField(enumObj.ToString());

            var attribArray = fieldInfo.GetCustomAttributes(false);

            if (attribArray.Length == 0)
            {
                return enumObj.ToString();
            }
            else
            {
                var attrib = attribArray[0] as DescriptionAttribute;
                return attrib.Description;
            }
        }
    }
}
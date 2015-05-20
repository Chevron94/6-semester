using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Media;

namespace BindingApplication
{
    class AgeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int age = (int)value;
            if (age < 10)
                return new SolidColorBrush(Colors.LightPink);
            else if (age < 20)
                return new SolidColorBrush(Colors.LightGreen);
            else if (age < 30)
                return new SolidColorBrush(Colors.LightBlue);
            else if (age < 40)
                return new SolidColorBrush(Colors.Green);
            else if (age < 50)
                return new SolidColorBrush(Colors.Yellow);
            else if (age < 60)
                return new SolidColorBrush(Colors.LightYellow);
            else if (age < 70)
                return new SolidColorBrush(Colors.GreenYellow);
            else if (age < 80)
                return new SolidColorBrush(Colors.Red);
            else if (age < 90)
                return new SolidColorBrush(Colors.PaleVioletRed);
            else return new SolidColorBrush(Colors.LightGoldenrodYellow);
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new Exception("The method or operation is not implemented.");
        }

         
    }
    
    public class EnabledConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string first = (string)values[0];
            string second = (string)values[1];
            string third = (string)values[2];
            string fourth = (string)values[3];
            string fives = (string)values[4];
            return first.Trim().Length != 0 &&
                                second.Trim().Length != 0 &&
                                third.Trim().Length != 0 &&
                                fourth.Trim().Length != 0 &&
                                fives.Trim().Length != 0;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new Exception("The method or operation is not implemented.");
        }


        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}

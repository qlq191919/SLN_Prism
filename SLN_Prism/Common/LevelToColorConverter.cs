using SLN_Prism.Common.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SLN_Prism.Common
{
    public class LevelToColorConverter : IValueConverter
    {
        /// <summary>
        /// 字符串转颜色，实际在XAML中使用触发器达到了同样的效果
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            
                Alarms alarms = new Alarms();
            alarms.level = value.ToString();
                switch (alarms.level)
                {
                    case "信息":
                        return Color.White;
                    case "警告":
                        return Color.Orange;
                    case "故障":
                        return Color.Red;
                        default:
                            return Color.Red;
                }
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SLN_Prism.Tool
{
   public class valueJudg
    {
        /// <summary>
        /// 判断当前是否为32位整数
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public bool intOk(string num)
        {
            bool result;
            int test = 0;
            if (Int32.TryParse(num, out test)) result = true;
            else result = false;

            return result;
        }

        /// <summary>
        /// 判断当前是否为双精度浮点数
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public bool doubleOk(string num)
        {
            bool result;
            double test = 0;
            if (Double.TryParse(num, out test)) result = true;
            else result = false;

            return result;
        }

        /// <summary>
        /// 判断当前是否为布尔量
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public bool BoolOk(string num)
        {
            bool result;
            bool test = false;
            if (Boolean.TryParse(num, out test)) result = true;
            else result = false;

            return result;
        }

        /// <summary>
        /// 判断当前是否为单精度浮点数
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public bool singleOk(string num)
        {
            bool result;
            Single test = 0;
            if (Single.TryParse(num, out test)) result = true;
            else result = false;

            return result;
        }

        /// <summary>
        /// 判断当前是否为16位整数
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public bool shortOk(string num)
        {
            bool result;
            short test = 0;
            if (Int16.TryParse(num, out test)) result = true;
            else result = false;

            return result;
        }

        /// <summary>
        /// 判断当前是否为非负16位整数
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public bool unShortOk(string num)
        {
            bool result;
            ushort test = 0;
            if (UInt16.TryParse(num, out test)) result = true;
            else result = false;

            return result;
        }

        /// <summary>
        /// 判断当前字符串是否可以转换为时间(内容)
        /// </summary>
        /// <param name="datetime">内容</param>
        /// <returns></returns>
        public bool datetimeOk(string datetime)
        {
            bool result;
            DateTime dt;
            if (DateTime.TryParse(datetime, out dt)) result = true;
            else result = false;

            return result;
        }

        /// <summary>
        /// 判断文本是否存在特殊字符
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>

        public bool IsSpecialChar(string str)
        {
            Regex regExp = new Regex("[ \\[ \\] \\^ _*×――(^)$%~!＠@＃#$…&%￥—+=<>《》!！??？:：•`·、。，；,.;/\'\"{}（）‘’“”]");
            if (regExp.IsMatch(str))
            {
                return true;
            }
            return false;
        }

    }
}

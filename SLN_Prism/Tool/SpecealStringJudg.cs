using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLN_Prism.Tool
{
   public class SpecealStringJudg
    {
        /// <summary>
        /// 是否存在特殊字符
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public bool IsSpecealStringExist(string txt)
        {
            bool IsExis = false;

            if (txt.Contains('?')
             || txt.Contains('/')
             || txt.Contains('*')
             || txt.Contains('@')
             || txt.Contains('&')
             || txt.Contains('$')
             || txt.Contains('!')
             || txt.Contains('^')
             || txt.Contains('<')
             || txt.Contains('>')
             || txt.Contains('{')
             || txt.Contains('}')
             || txt.Contains('+')
             || txt.Contains('-'))
            {
                IsExis = true;
            }

            return IsExis;
        }
    }
}

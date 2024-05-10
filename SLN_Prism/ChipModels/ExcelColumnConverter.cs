using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLN_Prism.ChipModels
{
    public static class ExcelColumnConverter
    {
        public static int ColumnLetterToNumber(string columnLetter)
        {
            int result = 0;
            for (int i = 0; i < columnLetter.Length; i++)
            {
                result *= 26;
                result += (columnLetter[i] - 'A' + 1);
            }
            return result;
        }

        public static string ColumnNumberToLetter(int columnNumber)
        {
            string result = "";
            while (columnNumber > 0)
            {
                int remainder = (columnNumber - 1) % 26;
                char letter = (char)('A' + remainder);
                result = letter + result;
                columnNumber = (columnNumber - 1) / 26;
            }
            return result;
        }
    }
}

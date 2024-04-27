using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SLN_Prism.Tool
{
        /*
        类库名称：INI文件读写操作
        开发日期：20200504-08
        修改日期：20200504-08
        类库说明：

        IniFiles ini = new IniFiles(Application.StartupPath + @"\MyConfig.INI");//Application.StartupPath只适用于winform窗体程序
        ini.WriteValue("登入详细", "账号", "test");
        ini.WriteValue("登入详细", "密码", "password");
        if (ini.ExistFile())//验证是否存在文件，存在就读取
        label1.Text = ini.ReadValue("登入详细", "用户名");
        */
    public class IniFiles
    {

        //声明API函数

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        /// <summary> 
        /// 构造方法 
        /// </summary> 
        /// <param name="INIPath">文件路径</param> 
        public IniFiles()
        {

        }


        /// <summary> 
        /// 写入INI文件 
        /// </summary> 
        /// <param name="Section">项目名称(如 [TypeName] )</param> 
        /// <param name="Key">键</param> 
        /// <param name="Value">值</param> 
        public void WriteValue(string Section, string Key, string Value, string inipath)
        {
            WritePrivateProfileString(Section, Key, Value, inipath);
        }
        /// <summary> 
        /// 读出INI文件 
        /// </summary> 
        /// <param name="Section">项目名称(如 [TypeName] )</param> 
        /// <param name="Key">键</param> 
        public string ReadValue(string Section, string Key, string inipath)
        {
            StringBuilder temp = new StringBuilder(500);
            int i = GetPrivateProfileString(Section, Key, "", temp, 500, inipath);
            return temp.ToString();
        }
        /// <summary> 
        /// 验证文件是否存在 
        /// </summary> 
        /// <returns>布尔值</returns> 
        public bool ExistFile(string inipath)
        {
            return File.Exists(inipath);
        }

        /// <summary>
        /// 读取INI文件全部信息
        /// </summary>
        /// <param name="inipath">路径</param>
        /// <returns></returns>
        public List<string> readAllValue(string inipath)
        {
            StreamReader sr = new StreamReader(inipath);
            List<string> listConfig = new List<string>();
            while (sr.Peek() > 0)
            {
                string str = sr.ReadLine();
                if (str.Contains("="))
                {
                    string[] split = str.Split('=');
                    listConfig.Add(split[1]);
                }
            }

            return listConfig;
        }
    }
}

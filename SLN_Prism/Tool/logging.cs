using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLN_Prism.Tool
{
   public class logging
    {
        /*名称:日志控制
         *开发时间:2024.4.27
         *修改时间:2024.4.27
         *说明:
         *1.构造函数(日志路径) 构造函数(日志路径)
         *2.日志写入(日志内容,日志类型) WriteLog(string log, string type)
         *3.读取范围内的日志文件(开始时间,结束时间)  DataTable readLogDt(DateTime startTime,DateTime endTime)
         *
         *日志类型为TXT格式
         */

        #region 私有变量

        private fileContrl fileCtrl = new fileContrl();//定义文件控制类
        private string filePath = "";

        #endregion

        #region 公有变量


        #endregion

        #region 公用方法
        /// <summary>
        /// 构造函数(日志路径)
        /// </summary>
        public logging(string filePath)
        {
            if (!fileCtrl.existFileInfo(filePath)) //判断是否存在日志文件夹
                fileCtrl.createDirectory(filePath); //不存在则创建

            this.filePath = filePath;

        }

        /// <summary>
        /// 日志写入(日志内容,日志类型)
        /// </summary>
        /// <param name="log"></param>
        /// <param name="type"></param>
        public void WriteLog(string log, string type)
        {
            string Time = DateTime.Now.ToString();
            string Type = type;
            string Info = log;

            System.IO.StreamWriter writer = null;
            try
            {

                string pathFile = filePath + string.Format("\\{0}.txt", DateTime.Now.ToString("yyyy-MM-dd"));

                writer = !System.IO.File.Exists(pathFile) ? System.IO.File.CreateText(pathFile) : System.IO.File.AppendText(pathFile); //判断文件是否存在，如果不存在则创建，存在则添加
                writer.WriteLine(string.Format("Logging|{0}|{1}|{2}", Time, Type, Info));

            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }
        }

        /// <summary>
        /// 读取范围内的日志文件(开始时间,结束时间)
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        public DataTable readLogDt(DateTime startTime, DateTime endTime)
        {
            DataTable loggingDt = new DataTable();//定义日志数据表
            loggingDt.Columns.Add("时间", Type.GetType("System.DateTime"));
            loggingDt.Columns.Add("级别");
            loggingDt.Columns.Add("信息");

            List<fileContrl.programmeFile> fileList = fileCtrl.readAllFileMessage(filePath);
            foreach (fileContrl.programmeFile file in fileList)
            {
                DateTime fileDateTime = Convert.ToDateTime(file.fileName);//定义临时时间，将文件名称转换为时间
                if (fileDateTime.Date >= startTime.Date && fileDateTime.Date <= endTime.Date)
                {
                    string[] allLine = fileCtrl.ReadAllLinesFile(filePath + "\\" + file.fileName + ".txt", Encoding.UTF8);//读取当前文件所有行

                    List<string> arrangeLine = new List<string>();
                    foreach (string line in allLine)
                    {
                        if (line.Contains("|"))
                            arrangeLine.Add(line);

                        if (!line.Contains("|"))
                        {
                            arrangeLine[arrangeLine.Count - 1] += line;
                        }
                    }

                    foreach (string lineText in arrangeLine)
                    {
                        if (lineText.Trim() != "")
                        {
                            string[] split = lineText.Split('|');//将行内容分隔
                            DataRow dr = loggingDt.NewRow();
                            dr["时间"] = split[1];
                            dr["级别"] = split[2];
                            dr["信息"] = split[3];
                            loggingDt.Rows.Add(dr);
                        }
                    }
                }
            }

            return loggingDt;
        }
        #endregion

    }
}


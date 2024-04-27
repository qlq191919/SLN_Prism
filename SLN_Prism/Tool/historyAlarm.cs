using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLN_Prism.Tool
{
  public  class historyAlarm
    {
        #region 私有变量

        private string path = "";//定义路径
        private fileContrl filectl = new fileContrl();//定义文件控制类
        private valueJudg judg = new valueJudg();//定义字符串判断类

        #endregion


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="path"></param>
        public historyAlarm(string path)
        {
            this.path = path;
        }

        /// <summary>
        /// 历史报警导入(路径)
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="historyText">历史内容</param>
        public void historyRecord(string historyText)
        {
            string timePath = path + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
            filectl.createFile(timePath);
            filectl.fileLineWrite(historyText, timePath);
        }

        /// <summary>
        /// 读取范围内的历史文件(开始时间,结束时间,路径)
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public DataTable readLogDt(DateTime startTime, DateTime endTime)
        {
            /*
             * 历史文本格式 例子如下:
             * ALM6|Z轴未回原点!|2020/12/12 16:54:28|2020/12/12 16:54:30|1374
             */
            DataTable historyDt = new DataTable();//定义历史报警数据表
            historyDt.Columns.Add("代码");
            historyDt.Columns.Add("开始时间", Type.GetType("System.DateTime"));
            historyDt.Columns.Add("结束时间", Type.GetType("System.DateTime"));
            historyDt.Columns.Add("内容");
            historyDt.Columns.Add("用时");

            List<fileContrl.programmeFile> fileList = filectl.readAllFileMessage(path);
            foreach (fileContrl.programmeFile file in fileList)
            {
                DateTime fileDateTime = Convert.ToDateTime(file.fileName);//定义临时时间，将文件名称转换为时间
                if (fileDateTime >= startTime.Date && fileDateTime <= endTime.Date)
                {
                    string[] allLine = filectl.ReadAllLinesFile(path + "\\" + file.fileName + ".txt", Encoding.UTF8);//读取当前文件所有行
                    foreach (string lineText in allLine)
                    {
                        if (lineText.Trim() != "")
                        {
                            string[] split = lineText.Split('|');//将行内容分隔
                            DataRow dr = historyDt.NewRow();
                            dr["代码"] = split[0];
                            dr["内容"] = split[1];

                            if (judg.doubleOk(split[4]))
                            {
                                dr["用时"] = (Convert.ToDouble(split[4]) / 1000).ToString("0.00") + "S";
                            }

                            dr["开始时间"] = judg.datetimeOk(split[2]) ? Convert.ToDateTime(split[2]) : Convert.ToDateTime("0001-01-01 00:00:00");
                            dr["结束时间"] = judg.datetimeOk(split[3]) ? Convert.ToDateTime(split[3]) : Convert.ToDateTime("0001-01-01 00:00:00");
                            historyDt.Rows.Add(dr);
                        }
                    }
                }
            }

            return historyDt;
        }
    }
}


using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLN_Prism.Tool
{
   public class alarm
    {
        /*名称:报警控制
         *开始时间:2024.4.27 
         *修改时间:2024.4.27
         *说明:
         *1.构造函数(路径) alarm(string path)
         *2.读取报警列表 List<nowAlarmList> readAlarm()
         *3.报警复位用时计算(开始时间,结束时间) int useTime(DateTime startTime, DateTime endTime)  
         *4.显示当前报警 DataTable showNowDtToUse(List<nowAlarmList> list)
         *5.报警触发控制(id号,触发状态) tiggerCtl(List<nowAlarmList> list,string id,bool statue)
         */

        #region 私有变量

        private string path = "";//定义路径
        private fileContrl filectl = new fileContrl();//定义文件控制类
        private valueJudg judg = new valueJudg();//定义字符串判断类

        #endregion

        #region 公用变量

        /// <summary>
        /// 当前报警结构体
        /// </summary>
        public struct nowAlarmList
        {
            /// <summary>
            /// 报警id号
            /// </summary>
            public string id;
            /// <summary>
            /// 报警内容
            /// </summary>
            public string text;
            /// <summary>
            /// 开始时间触发
            /// </summary>
            public bool startTigger;
            /// <summary>
            /// 结束时间触发
            /// </summary>
            public bool endTigger;
            /// <summary>
            /// 当前报警标志
            /// </summary>
            public bool flag;
            /// <summary>
            /// 开始时间
            /// </summary>
            public DateTime startTime;
            /// <summary>
            /// 结束时间
            /// </summary>
            public DateTime endTime;
            /// <summary>
            /// 地址
            /// </summary>
            public string adr;
        }

        #endregion

        #region 公有方法
        /// <summary>
        /// 构造函数(路径)
        /// </summary>
        /// <param name="path">路径</param>
        public alarm(string path)
        {
            this.path = path;


        }



        /// <summary>
        /// 读取报警列表
        /// </summary>
        /// <returns></returns>
        public List<nowAlarmList> readAlarm()
        {
            List<nowAlarmList> alarmList = new List<nowAlarmList>();//定义临时报警列表
            nowAlarmList nowAlarm = new nowAlarmList();//定义临时当前报警结构体
            string[] alarmLines = filectl.ReadAllLinesFile(path, Encoding.UTF8);//读取报警列表文件
            foreach (string line in alarmLines) //历遍所有报警行
            {
                /***************
                 * 报警格式  ALM1|内容|地址
                 **************/
                string[] split = line.Split('|');//临时定义分割文件，分割名称和内容
                nowAlarm.id = split[0];//赋值名称
                nowAlarm.text = split[1];//赋值内容
                nowAlarm.adr = split[2];//地址

                alarmList.Add(nowAlarm);
            }

            return alarmList;
        }

        /// <summary>
        /// 报警复位用时计算(开始时间,结束时间)
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        public int useTime(DateTime startTime, DateTime endTime)
        {
            //单位毫秒
            return (int)(endTime - startTime).TotalMilliseconds;
        }

        /// <summary>
        /// 显示当前报警
        /// </summary>
        /// <returns></returns>
        public DataTable showNowDtToUse(List<nowAlarmList> list)
        {
            DataTable showDt = new DataTable();//定义当前报警列表
            showDt.Columns.Add("时间", Type.GetType("System.DateTime"));
            showDt.Columns.Add("报警信息");

            foreach (nowAlarmList item in list)
            {
                if (item.flag)
                {
                    DataRow dr = showDt.NewRow();
                    dr["时间"] = item.startTime;
                    dr["报警信息"] = item.text;
                    showDt.Rows.Add(dr);
                }
            }

            return showDt;
        }

        /// <summary>
        /// 报警触发控制(id号,触发状态)
        /// </summary>
        /// <param name="id">id号</param>
        /// <param name="statue">触发状态</param>
        public void tiggerCtl(List<nowAlarmList> list, string id, bool statue)
        {
            nowAlarmList nowAlarm = new nowAlarmList();//定义临时当前报警结构体

            for (int i = 0; i < list.Count; i++)
            {
                nowAlarm = list[i];
                if (nowAlarm.id == id.ToUpper().Trim())
                {
                    if (statue)
                    {
                        nowAlarm.startTigger = true;
                        nowAlarm.flag = statue;
                    }
                    else
                    {
                        nowAlarm.endTigger = true;
                        nowAlarm.flag = statue;
                    }
                }
                list[i] = nowAlarm;

            }

        }


        #endregion
    
    }
}

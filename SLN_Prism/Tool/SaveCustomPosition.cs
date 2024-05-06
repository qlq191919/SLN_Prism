using SLN_Prism.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;


namespace SLN_Prism.Tool
{
    public  class SaveCustomPosition
    {
        private fileContrl fileCtrl = new fileContrl();//定义文件控制类
        private string filePath = "";
        public SaveCustomPosition(string filePath)
        {
            if (!fileCtrl.existFileInfo(filePath)) //判断是否存在文件夹
                fileCtrl.createDirectory(filePath); //不存在则创建

            this.filePath = filePath;
        }
        /// <summary>
        /// 写入预存坐标
        /// </summary>
        /// <param name="customPositionName"></param>
        /// <param name="customPosition"></param>
        /// <param name="axisIndex"></param>
        public void WriteCustomPosition(string customPositionName,double customPosition, int axisIndex)
        {

            
            int AxisIndex = axisIndex;

            System.IO.StreamWriter writer = null;
            try
            {

                string pathFile = filePath + string.Format("\\Axis[{0}]预存坐标.txt", axisIndex.ToString());

                writer = !System.IO.File.Exists(pathFile) ? System.IO.File.CreateText(pathFile) : System.IO.File.AppendText(pathFile); //判断文件是否存在，如果不存在则创建，存在则添加
                writer.WriteLine(string.Format("坐标|{0}|{1}|", customPositionName, customPosition.ToString()));
              
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
        /// 读取预存坐标到DataTable
        /// </summary>
        /// <param name="axisIndex"></param>
        /// <returns></returns>
               public DataTable ReadCustomPosition(int axisIndex)
        {
            DataTable PositionDt = new DataTable();//定义数据表
            PositionDt.Columns.Add("名称");
            PositionDt.Columns.Add("坐标");
            List<fileContrl.programmeFile> fileList = fileCtrl.readAllFileMessage(filePath);
            foreach (fileContrl.programmeFile file in fileList)
            {
               string Index =  System.Text.RegularExpressions.Regex.Replace(file.fileName, @"[^0-9]+", "");
                if (Index == axisIndex.ToString())
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
                            DataRow dr = PositionDt.NewRow();
                          
                            dr["名称"] = split[1];
                            dr["坐标"] = split[2];                          
                            PositionDt.Rows.Add(dr);
                        }
                    }
                }
            }

            return PositionDt;
        }
        /// <summary>
        /// 读取预存坐标到List
        /// </summary>
        /// <param name="axisIndex"></param>
        /// <returns></returns>
        public ObservableCollection<CustomPosition> ReadCustomPositionToList(int axisIndex)
        {
            ObservableCollection<CustomPosition> PositionLT = new ObservableCollection<CustomPosition>();//定义数据表
       
            List<fileContrl.programmeFile> fileList = fileCtrl.readAllFileMessage(filePath);
            foreach (fileContrl.programmeFile file in fileList)
            {
                string Index = System.Text.RegularExpressions.Regex.Replace(file.fileName, @"[^0-9]+", "");
                if (Index == axisIndex.ToString())
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


                            PositionLT.Add(new CustomPosition() { Name = split[1], Position = double.Parse(split[2]) });                        
                        }
                    }
                }
            }

            return PositionLT;
        }

    }
}

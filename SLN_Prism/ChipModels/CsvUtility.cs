using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLN_Prism.ChipModels
{

    public static class CsvUtility
    {
        public static void WriteLog(ChipInfo chipInfo, string path, string taskId)
        {
            try
            {
                string dirFolder = Path.Combine(path, "CSV", $"{DateTime.Now.Year}年", $"{DateTime.Now.Month}月");
                DirectoryInfo dirInfo = new DirectoryInfo(dirFolder);
                dirInfo.Create();
                string fileName = Path.Combine(dirFolder, $"TaskId{taskId}{DateTime.Today:yyyyMMdd}.csv");
                if (!File.Exists(fileName))
                {
                    using var writer = new StreamWriter(fileName, false, new UTF8Encoding(true));
                    writer.AutoFlush = true;
                    BuildHeader(writer);
                    BuildContent(chipInfo, writer);
                    writer.Flush();
                    writer.BaseStream.Flush();
                }
                else
                {
                    using var writer = new StreamWriter(fileName, true);
                    writer.AutoFlush = true;
                    BuildContent(chipInfo, writer);
                    writer.Flush();
                    writer.BaseStream.Flush();
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Info($"Csv存储出错：{ex.Message}");
            }
        }

        private static void BuildContent(ChipInfo chipInfo, StreamWriter writer)
        {
            var content = new StringBuilder();
            content.Append($"{chipInfo.Name},{chipInfo.Time},{chipInfo.Id},{chipInfo.Pressure},{chipInfo.Result},{chipInfo.RowColumn},{chipInfo.X},{chipInfo.Y},{chipInfo.Angle}");
            writer.WriteLine(content);
        }

        private static void BuildHeader(StreamWriter writer)
        {
            var header1 = new StringBuilder();

            header1.Append($@"芯片名称,测试时间,芯片Id,压力值,测试结果,所在行列数,芯片坐标X,芯片坐标Y,芯片角度");
            writer.WriteLine(header1);
        }
    }
}


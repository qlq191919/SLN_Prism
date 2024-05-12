using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog.Config;
using SLN_Prism.Common.Models;
using SLN_Prism.ViewModels;
using Prism.Mvvm;

namespace SLN_Prism.Common
{
    public class LoggerHelper:BindableBase
    {
        private static readonly Lazy<ILogger> LazyLogger = new Lazy<ILogger>(LogManager.GetCurrentClassLogger);

        public static ILogger Default => LazyLogger.Value;

        public static void Info(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                Default.Info(message);
                Alarms a = new Alarms();
                a.message = message;
                a.level = "信息";
                a.time = DateTime.Now;
                HomeViewModel.LogText.Add(a);
            }
        }


        public static void Warn(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                Default.Warn(message);
                Alarms a = new Alarms();
                a.message = message;
                a.level = "警告";
                a.time = DateTime.Now;
                HomeViewModel.LogText.Add(a);
            }
        }

        public static void Debug(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                Default.Debug(message);
                
            }
        }

        public static void Error(string message, Exception exception = null)
        {
            if (!string.IsNullOrEmpty(message) || exception != null)
            {
                if (exception == null)
                {
                    Default.Error(message);
                    Alarms a = new Alarms();
                    a.message = message;
                    a.level = "故障";
                    a.time = DateTime.Now;
                    HomeViewModel.LogText.Add(a);
                }
                else
                {
                    Default.Error(exception, message);
                }
            }
        }
    }
}

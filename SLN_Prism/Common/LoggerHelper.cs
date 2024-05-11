using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog.Config;

namespace SLN_Prism.Common
{
    public class LoggerHelper
    {
        private static readonly Lazy<ILogger> LazyLogger = new Lazy<ILogger>(LogManager.GetCurrentClassLogger);

        public static ILogger Default => LazyLogger.Value;

        public static void Info(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                Default.Info(message);
            }
        }

        public static void Warn(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                Default.Warn(message);
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
                }
                else
                {
                    Default.Error(exception, message);
                }
            }
        }
    }
}

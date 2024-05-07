using cszmcaux;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLN_Prism.Zmotion
{
    public class ZmotionFun
    {/// <summary>
     /// 创建单例, 避免多次实例化, 节省内存，同时确保每次调用都是同一个实例
     /// </summary>
        public static ZmotionFun zmotionFun { get; } = new ZmotionFun();  
        /// <summary>
        /// 线程锁，用于保证多线程安全
        /// </summary>
        private readonly object _lockerMotionCard = new object();
        /// <summary>
        /// ZMotion 控制卡句柄
        /// </summary>
        public IntPtr ZMotionCardHandle { get; private set; }
        /// <summary>
        /// 控制器返回的错误代码
        /// </summary>
        public int ErrorCode { get; private set; }
        /// <summary>
        /// 控制器返回的错误信息
        /// </summary>
        public string ErrorMessage { get; private set; }
        /// <summary>
        /// 连接状态，自动实现属性
        /// </summary>
        public bool IsConnected => ZMotionCardHandle != IntPtr.Zero;

        #region 方法
        #region 连接与断开
        public bool Connect(string ip)  //连接控制器
        {
            if (IsConnected) return true;  //若已连接，则直接返回true            
            {
                lock (_lockerMotionCard)  //线程锁
                {
                    ErrorCode = zmcaux.ZAux_OpenEth(ip, out IntPtr handle);  //打开控制器
                    ZMotionCardHandle = handle;  //保存句柄
                }
            }
            return IsConnected;  //返回连接状态
        }

        public void Close()
        {
            if (!IsConnected) return;

            lock (_lockerMotionCard)
            {
                ErrorCode =zmcaux.ZAux_Close(ZMotionCardHandle);  //关闭控制器
                ZMotionCardHandle = IntPtr.Zero;  //清空句柄
            }
        }

        public bool ReConnect(string ip)  //强制连接控制器
        {
            Close();  //先关闭已有的连接
            return Connect(ip);  //再重新连接
        }
    }
}

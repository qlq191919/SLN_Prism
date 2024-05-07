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
        #endregion
        /// <summary>
        /// 发送字符串命令到控制器，缓存方式(当控制器没有缓冲时自动阻塞)。
        /// </summary>
        /// <param name="command"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public bool Execute(string command, StringBuilder str)
        {
            ErrorCode = zmcaux.ZAux_Execute(ZMotionCardHandle, command, str, 1000);
            return ErrorCode == 0;
        }
        #region 基本轴参数设置和获取

        /// <summary>
        /// 设置单轴类型
        /// </summary>
        /// <param name="axisIndex">轴号</param>
        /// <param name="axisType">轴类型</param>
        /// <returns>操作结果。若设置成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</returns>
        public bool SetAxisType(int axisIndex, AxisType axisType)
        {
            ErrorCode = zmcaux.ZAux_Direct_SetAtype(ZMotionCardHandle, axisIndex, (int)axisType);
            return ErrorCode == 0;
        }
        /// <summary>
        /// 设置单轴脉冲当量
        /// </summary>
        /// <param name="axisIndex">轴号</param>
        /// <param name="units">轴脉冲当量</param>
        /// <returns>操作结果。若设置成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</returns>
        public bool SetAxisUnits(int axisIndex, float units)
        {
            ErrorCode = zmcaux.ZAux_Direct_SetUnits(ZMotionCardHandle, axisIndex, units);
            return ErrorCode == 0;
        }
        /// <summary>
        /// 设置单轴速度
        /// </summary>
        /// <param name="axisIndex">轴号</param>
        /// <param name="speed">轴速度</param>
        /// <returns>操作结果。若设置成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</returns>
        public bool SetAxisSpeed(int axisIndex, float speed)
        {
            ErrorCode = zmcaux.ZAux_Direct_SetSpeed(ZMotionCardHandle, axisIndex, speed);
            return ErrorCode == 0;
        }

        /// <summary>
        /// 设置单轴加速度
        /// </summary>
        /// <param name="axisIndex">轴号</param>
        /// <param name="accel">轴加速度</param>
        /// <returns>操作结果。若设置成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</returns>
        public bool SetAxisAccelSpeed(int axisIndex, float accel)
        {
            ErrorCode = zmcaux.ZAux_Direct_SetAccel(ZMotionCardHandle, axisIndex, accel);
            return ErrorCode == 0;
        }
        /// <summary>
        /// 设置单轴减速度
        /// </summary>
        /// <param name="axisIndex">轴号</param>
        /// <param name="decel">轴减速度</param>
        /// <returns>操作结果。若设置成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</returns>
        public bool SetAxisDecelSpeed(int axisIndex, float decel)
        {
            ErrorCode = zmcaux.ZAux_Direct_SetDecel(ZMotionCardHandle, axisIndex, decel);
            return ErrorCode == 0;
        }
        /// <summary>
        /// 获取单轴脉冲当量
        /// </summary>
        /// <param name="axisIndex">轴号</param>
        /// <param name="units">轴脉冲当量</param>
        /// <returns>操作结果。若获取成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</returns>
        public bool GetAxisUnits(int axisIndex, out float units)
        {
            units = float.NaN;
            ErrorCode = zmcaux.ZAux_Direct_GetUnits(ZMotionCardHandle, axisIndex, ref units);
            return ErrorCode == 0;
        }
        /// <summary>
        /// 获取单轴速度
        /// </summary>
        /// <param name="axisIndex">轴号</param>
        /// <param name="speed">轴速度</param>
        /// <returns>操作结果。若获取成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</returns>
        public bool GetAxisSpeed(int axisIndex, out float speed)
        {
            speed = float.NaN;
            ErrorCode = zmcaux.ZAux_Direct_GetSpeed(ZMotionCardHandle, axisIndex, ref speed);
            return ErrorCode == 0;
        }
        /// <summary>
        /// 获取轴的Speed和MSpeed
        /// </summary>
        /// <param name="axisIndex">轴号</param>
        /// <param name="speed">speed</param>
        /// <param name="mSpeed">mSpeed</param>
        /// <returns>操作结果。若获取成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</returns>
        public bool GetAxisSpeed(int axisIndex, out float speed, out float mSpeed)
        {
            mSpeed = float.NaN;
            return GetAxisSpeed(axisIndex, out speed) && GetAxisMSpeed(axisIndex, out mSpeed);
        }
        /// <summary>
        /// 获取单轴加速度
        /// </summary>
        /// <param name="axisIndex">轴号</param>
        /// <param name="accel">轴加速度</param>
        /// <returns>操作结果。若获取成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</returns>
        public bool GetAxisAccel(int axisIndex, out float accel)
        {
            accel = float.NaN;
            ErrorCode = zmcaux.ZAux_Direct_GetAccel(ZMotionCardHandle, axisIndex, ref accel);
            return ErrorCode == 0;
        }
        /// <summary>
        /// 获取单轴减速度
        /// </summary>
        /// <param name="axisIndex">轴号</param>
        /// <param name="decel">轴减速度</param>
        /// <returns>操作结果。若获取成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</returns>
        public bool GetAxisDecel(int axisIndex, out float decel)
        {
            decel = float.NaN;
           ErrorCode =zmcaux.ZAux_Direct_GetDecel(ZMotionCardHandle, axisIndex, ref decel);
            return ErrorCode == 0;
        }

        #endregion 基本轴参数设置和获取

#endregion 方法
    }
}

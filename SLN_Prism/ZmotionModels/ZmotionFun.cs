using cszmcaux;
using SLN_Prism.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLN_Prism.ZmotionModels
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
        public bool GetAxisAccelSpeed(int axisIndex, out float accel)
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
        public bool GetAxisDecelSpeed(int axisIndex, out float decel)
        {
            decel = float.NaN;
           ErrorCode =zmcaux.ZAux_Direct_GetDecel(ZMotionCardHandle, axisIndex, ref decel);
            return ErrorCode == 0;
        }
        /// <summary>
        /// 获取单轴S曲线时间
        /// </summary>
        /// <param name="axisIndex">轴号</param>
        /// <param name="sramp">S曲线时间，单位ms（为0时则表示梯形加减速）</param>
        /// <returns>操作结果。若获取成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</returns>
        public bool GetAxisSrampSpeed(int axisIndex, out float sramp)
        {
            sramp = float.NaN;
            //ZAux_Direct_GetSramp，获取单轴S曲线时间，参数分别为连接句柄、轴号、返回S曲线时间
           ErrorCode = zmcaux.ZAux_Direct_GetSramp(ZMotionCardHandle, axisIndex, ref sramp);
            return ErrorCode == 0;
        }

        #endregion 基本轴参数设置和获取

        #region 特殊IO设置和获取
        /// <summary>
        /// 设置单轴原点信号输入口（设置了原点开关后，ZMC控制器输入OFF时认为有信号输入，要相反效果可以用INVERT_IN反转电平(ECI系列控制器除外)。）
        /// </summary>
        /// <param name="axisIndex">轴号</param>
        /// <param name="ioPort">IO口编号（为-1时表示取消设置）</param>
        /// <returns>操作结果。若设置成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</returns>
        public bool SetAxisHomeIOIn(int axisIndex, int ioPort)
        {            
            ErrorCode = zmcaux.ZAux_Direct_SetDatumIn(ZMotionCardHandle, axisIndex, ioPort);
            return ErrorCode == 0;
        }
        /// <summary>
        /// 获取单轴原点信号输入口
        /// </summary>
        /// <param name="axisIndex">轴号</param>
        /// <param name="ioPort">IO口编号（为-1时表示没有设置）</param>
        /// <returns>操作结果。若获取成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</returns>
        public bool GetAxisHomeIOIn(int axisIndex, out int ioPort)
        {
            ioPort = int.MinValue;           
           ErrorCode = zmcaux.ZAux_Direct_GetDatumIn(ZMotionCardHandle, axisIndex, ref ioPort);
            return ErrorCode == 0;
        }

        /// <summary>
        /// 设置单轴正向限位信号输入口
        /// </summary>
        /// <param name="axisIndex">轴号</param>
        /// <param name="ioPort">IO口编号（为-1时表示取消设置）</param>
        /// <returns>操作结果。若设置成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</returns>
        public bool SetAxisForWardLimitIOIn(int axisIndex, int ioPort)
        {            
            ErrorCode = zmcaux.ZAux_Direct_SetFwdIn(ZMotionCardHandle, axisIndex, ioPort);
            return ErrorCode == 0;
        }
        /// <summary>
        /// 获取单轴正向限位信号输入口
        /// </summary>
        /// <param name="axisIndex">轴号</param>
        /// <param name="ioPort">IO口编号（为-1时表示没有设置）</param>
        /// <returns>操作结果。若获取成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</returns>
        public bool GetAxisForewardLimitIOIn(int axisIndex, out int ioPort)
        {
            ioPort = int.MinValue;
            ErrorCode = zmcaux.ZAux_Direct_GetFwdIn(ZMotionCardHandle, axisIndex, ref ioPort);
            return ErrorCode == 0;
        }
        /// <summary>
        /// 设置单轴负向限位信号输入口
        /// </summary>
        /// <param name="axisIndex">轴号</param>
        /// <param name="ioPort">IO口编号（为-1时表示取消设置）</param>
        /// <returns>操作结果。若设置成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</returns>
        public bool SetAxisBackwardLimitIOIn(int axisIndex, int ioPort)
        {           
            ErrorCode = zmcaux.ZAux_Direct_SetRevIn(ZMotionCardHandle, axisIndex, ioPort);
            return ErrorCode == 0;
        }
        /// <summary>
        /// 获取单轴负向限位信号输入口
        /// </summary>
        /// <param name="axisIndex">轴号</param>
        /// <param name="ioPort">IO口编号（为-1时表示没有设置）</param>
        /// <returns>操作结果。若获取成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</returns>
        public bool GetAxisBackwardLimitIOIn(int axisIndex, out int ioPort)
        {
            ioPort = int.MinValue;           
            ErrorCode = zmcaux.ZAux_Direct_GetRevIn(ZMotionCardHandle, axisIndex, ref ioPort);
            return ErrorCode == 0;
        }
        /// <summary>
        /// 设置单轴伺服报警信号输入口
        /// </summary>
        /// <param name="axisIndex">轴号</param>
        /// <param name="ioPort">IO口编号（为-1时表示取消设置）</param>
        /// <returns>操作结果。若设置成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</returns>
        public bool SetAxisAlarmIOIn(int axisIndex, int ioPort)
        {
            //ZAux_Direct_SetAlmIn，设置单轴伺服报警信号输入口，参数分别为连接句柄、轴号、IO口编号
            //注意：ECI系列控制器不支持报警输入，故该函数无效。
            ErrorCode = zmcaux.ZAux_Direct_SetAlmIn(ZMotionCardHandle, axisIndex, ioPort);
            return ErrorCode == 0;
        }
        /// <summary>
        /// 获取单轴报警信号输入口
        /// </summary>
        /// <param name="axisIndex">轴号</param>
        /// <param name="ioPort">IO口编号（为-1时表示没有设置）</param>
        /// <returns>操作结果。若获取成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</returns>
        public bool GetAxisAlarmIOIn(int axisIndex, out int ioPort)
        {
            ioPort = int.MinValue;          
            ErrorCode = zmcaux.ZAux_Direct_GetAlmIn(ZMotionCardHandle, axisIndex, ref ioPort);
            return ErrorCode == 0;
        }
        #endregion 特殊IO设置和获取

        #region 轴状态和参数设置与获取
        /// <summary>
        /// 获取单轴运动状态（是否空闲）
        /// </summary>
        /// <param name="axisIndex">轴号</param>
        /// <param name="isIdle">轴的运动状态。（true：运动完成；false：运动中）</param>
        /// <returns>操作结果。若获取成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</returns>
        public bool GetAxisIsIdle(int axisIndex, out bool isIdle)
        {
            isIdle = false;
            int rt = int.MinValue;
            //ZAux_Direct_GetIfIdle，获取单轴运动状态，参数分别为连接句柄、轴号、返回轴运动状态-1为空闲0为运动中
           ErrorCode = zmcaux.ZAux_Direct_GetIfIdle(ZMotionCardHandle, axisIndex, ref rt);
            if (rt == -1) isIdle = true;
            return ErrorCode == 0;
        }

        /// <summary>
        /// 获取单轴是否还有运动缓冲（除当前运动）
        /// </summary>
        /// <param name="axisIndex">轴号</param>
        /// <param name="isFinished">轴的缓冲状态。（true：没有剩余运动缓冲；false：还有剩余运动缓冲）</param>
        /// <returns>操作结果。若获取成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</returns>
        public bool GetAxisLoadedStatus(int axisIndex, out bool isFinished)
        {
            isFinished = false;
            int rt = int.MinValue;
            //ZAux_Direct_GetLoaded，获取单轴是否还有运动缓冲，参数分别为连接句柄、轴号、返回轴是否还有运动缓冲-1为没有缓冲0为有缓冲
           ErrorCode = zmcaux.ZAux_Direct_GetLoaded(ZMotionCardHandle, axisIndex, ref rt);
            if (rt == -1) isFinished = true;
            return ErrorCode == 0;
        }
        /// <summary>
        /// 获取单轴指令位置（DPos）
        /// </summary>
        /// <param name="axisIndex">轴号</param>
        /// <param name="val">轴的指令位置（DPos）</param>
        /// <returns>操作结果。若操作成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</returns>
        public bool GetAxisDPosition(int axisIndex, out float val)
        {
            val = float.NaN;
            ErrorCode =zmcaux.ZAux_Direct_GetDpos(ZMotionCardHandle, axisIndex, ref val);
            return ErrorCode == 0;
        }
        /// <summary>
        /// 设置单轴指令位置（DPos）
        /// </summary>
        /// <param name="axisIndex">轴号</param>
        /// <param name="val">轴的指令位置（DPos）</param>
        /// <returns>操作结果。若操作成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</returns>
        public bool SetAxisDPosition(int axisIndex, float val)
        {
            ErrorCode = zmcaux.ZAux_Direct_SetDpos(ZMotionCardHandle, axisIndex, val);
            return ErrorCode == 0;
        }

        /// <summary>
        /// 获取单轴编码器反馈位置（MPos）
        /// </summary>
        /// <param name="axisIndex">轴号</param>
        /// <param name="val">轴的编码器反馈位置（MPos）</param>
        /// <returns>操作结果。若操作成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</returns>
        public bool GetAxisMPosition(int axisIndex, out float val)
        {
            val = float.NaN;
           ErrorCode =zmcaux.ZAux_Direct_GetMpos(ZMotionCardHandle, axisIndex, ref val);
            return ErrorCode == 0;
        }
        /// <summary>
        /// 设置单轴编码器反馈位置（MPos）
        /// </summary>
        /// <param name="axisIndex">轴号</param>
        /// <param name="val">轴的编码器反馈位置（MPos）</param>
        /// <returns>操作结果。若操作成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</returns>
        public bool SetAxisMPosition(int axisIndex, float val)
        {
            ErrorCode = zmcaux.ZAux_Direct_SetMpos(ZMotionCardHandle, axisIndex, val);
            return ErrorCode == 0;
        }
        /// <summary>
        /// 获取单轴的位置信息（DPos和MPos）
        /// </summary>
        /// <param name="axisIndex">轴号</param>
        /// <param name="dpos">轴的指令位置（DPos）</param>
        /// <param name="mpos">轴的编码器反馈位置（MPos）</param>
        /// <returns>操作结果。若操作成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</returns>
        public bool GetAxisPosition(int axisIndex, out float dpos, out float mpos)
        {
            mpos = float.NaN;
            //返回轴指令位置和编码器反馈位置
            return GetAxisDPosition(axisIndex, out dpos) && GetAxisMPosition(axisIndex, out mpos);
        }
        /// <summary>
        /// 获取单轴的指定参数值
        /// </summary>
        /// <param name="axisIndex">轴号</param>
        /// <param name="key">参数名称</param>
        /// <param name="val">参数值</param>
        /// <returns>操作结果。若操作成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</returns>
        public bool GetAxisParameter(int axisIndex, string key, out float val)
        {
            val = float.NaN;
            //ZAux_Direct_GetParam，获取单轴指定参数值，参数分别为连接句柄、参数名称、轴号、输出参数值
            ErrorCode = zmcaux.ZAux_Direct_GetParam(ZMotionCardHandle, key, axisIndex, ref val);
            return ErrorCode == 0;
        }
        /// <summary>
        /// 设置单轴的指定参数值
        /// </summary>
        /// <param name="axisIndex">轴号</param>
        /// <param name="key">参数名称</param>
        /// <param name="val">参数值</param>
        /// <returns>操作结果。若操作成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</returns>
        public bool SetAxisParameter(int axisIndex, string key, float val)
        {
            //ZAux_Direct_SetParam，设置单轴指定参数值，参数分别为连接句柄、参数名称、轴号、参数值
            ErrorCode = zmcaux.ZAux_Direct_SetParam(ZMotionCardHandle, key, axisIndex, val);
            return ErrorCode == 0;
        }
        /// <summary>
        /// 获取指定数量的多个轴的指定参数值
        /// </summary>
        /// <param name="axisNumber">轴数量</param>
        /// <param name="key">参数名称</param>
        /// <param name="vals">参数值数组</param>
        /// <returns>操作结果。若操作成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</returns>
        public bool GetAxisParameter(int axisNumber, string key, out float[] vals)
        {
            vals = new float[axisNumber];
            //ZAux_Direct_GetAllAxisPara，获取指定数量的多个轴的指定参数值，参数分别为连接句柄、参数名称、轴数量、输出参数值数组
           ErrorCode = zmcaux.ZAux_Direct_GetAllAxisPara(ZMotionCardHandle, key, axisNumber, vals);
            return ErrorCode == 0;
        }
        /// <summary>
        /// 获取单轴快速减速度
        /// </summary>
        /// <param name="axisIndex">轴号</param>
        /// <param name="val">轴的快速减速度</param>
        /// <returns>操作结果。若操作成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</returns>
        public bool GetAxisFastDecel(int axisIndex, out float val)
        {
            val = float.NaN;
            //ZAux_Direct_GetFastDec，获取单轴快速减速度，参数分别为连接句柄、轴号、返回轴快速减速度
           ErrorCode = zmcaux.ZAux_Direct_GetFastDec(ZMotionCardHandle, axisIndex, ref val);
            return ErrorCode == 0;
        }
        /// <summary>
        /// 设置单轴快速减速度
        /// </summary>
        /// <param name="axisIndex">轴号</param>
        /// <param name="val">轴的快速减速度</param>
        /// <returns>操作结果。若操作成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</returns>
        public bool SetAxisFastDecel(int axisIndex, float val)
        {
            //ZAux_Direct_SetFastDec，设置单轴快速减速度，参数分别为连接句柄、轴号、轴快速减速度
           ErrorCode = zmcaux.ZAux_Direct_SetFastDec(ZMotionCardHandle, axisIndex, val);
            return ErrorCode == 0;
        }
        /// <summary>
        /// 获取单轴起始速度
        /// </summary>
        /// <param name="axisIndex">轴号</param>
        /// <param name="val">轴的起始速度</param>
        /// <returns>操作结果。若操作成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</returns>
        public bool GetAxisLSpeed(int axisIndex, out float val)
        {
            val = float.NaN;
            ErrorCode = zmcaux.ZAux_Direct_GetLspeed(ZMotionCardHandle, axisIndex, ref val);
            return ErrorCode == 0;
        }

        /// <summary>
        /// 设置单轴起始速度
        /// </summary>
        /// <param name="axisIndex">轴号</param>
        /// <param name="val">轴的起始速度</param>
        /// <returns>操作结果。若操作成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</returns>
        public bool SetAxisLSpeed(int axisIndex, float val)
        {
            ErrorCode = zmcaux.ZAux_Direct_SetLspeed(ZMotionCardHandle, axisIndex, val);
            return ErrorCode == 0;
        }

        /// <summary>
        /// 获取轴最大速度（指实际脉冲频率，不是单纯的Speed值）
        /// </summary>
        /// <param name="axisIndex">轴号</param>
        /// <param name="val">轴的最大速度</param>
        /// <returns>操作结果。若操作成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</returns>
        public bool GetAxisMaxSpeed(int axisIndex, out int val)
        {
            //ZAux_Direct_GetMaxSpeed，获取轴最大速度，参数分别为连接句柄、轴号、返回轴最大速度
            //1.一旦发现超过此设置值会强制，并且产生轴告警
            //2.对编码器轴，设置值低于 500K 时会启用编码器滤波，设置值高于 1M时会取消编码器滤波设置。
            //默认值为 1000000（老固件的默认脉冲频率为500000）。
            //3.使用直线电机速度较快时，一般容易脉冲频率超限，可把数值适当设置小一点。
            val = int.MinValue;
            ErrorCode = zmcaux.ZAux_Direct_GetMaxSpeed(ZMotionCardHandle, axisIndex, ref val);
            return ErrorCode == 0;
        }

        /// <summary>
        /// 设置轴最大速度（指实际脉冲频率，不是单纯的Speed值）
        /// </summary>
        /// <param name="axisIndex">轴号</param>
        /// <param name="val">轴的最大速度</param>
        /// <returns>操作结果。若操作成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</returns>
        public bool SetAxisMaxSpeed(int axisIndex, int val)
        {
            ErrorCode = zmcaux.ZAux_Direct_SetMaxSpeed(ZMotionCardHandle, axisIndex, val);
            return ErrorCode == 0;
        }
        #endregion 轴状态和参数设置与获取

        #region 基本运动控制

        #region 点位运动和直线插补
        /// <summary>
        /// 急停（所有轴立即停止）
        /// Mode0~2减速度按FASTDEC和DECEL中最大的值。
        /// EmergencyStop后要调用绝对位置运动，必须先
        /// IDLE 等待停止完成。
        /// </summary>
        /// <param name="mode">停止模式</param>
        /// <returns>操作结果。若操作成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</returns>
        public bool EmergencyStop()
        {
            //ZAux_Direct_EmergencyStop，急停，参数分别为连接句柄、停止模式,0为取消当前运动，1为取消缓冲的运动，2为0+1，3为立即停止脉冲
            ErrorCode = zmcaux.ZAux_Direct_Rapidstop(ZMotionCardHandle, (int)MotionCancelMode.InterruptPulse);
            for (int i = 0; i < 13; i++)
            {
                ZmotionFun.zmotionFun.BusCmd_SetAxisEnable(i, false); //断使所有轴
            }
            ZmotionFun.zmotionFun.SetIoOut(12, IoState.Off);
            LoggerHelper.Info("急停完毕，所有轴断使能");     //用LoggerHelper类的Info方法记录了一条日志消息，用于标识急停操作已完成
            return ErrorCode == 0;
        }

        /// <summary>
        /// 重新使能
        /// </summary>
        /// <returns></returns>
        public bool AxesAllEnable()
        {
            LoggerHelper.Info("执行轴全部使能指令"); //用LoggerHelper类的Info方法记录了一条日志消息，用于标识轴全部使能指令已执行
            for (int i = 0; i < 13; i++)
            {
                ZmotionFun.zmotionFun.BusCmd_SetAxisEnable(i, true); //使能所有轴
            }
            ZmotionFun.zmotionFun.SetIoOut(12, IoState.On);
            return ErrorCode == 0;
        }

        /// <summary>
        /// 停止
        /// </summary>
        /// <returns></returns>
        public bool StopRun()
        {
            ErrorCode = zmcaux.ZAux_Direct_Rapidstop(ZMotionCardHandle, (int)MotionCancelMode.InterruptPulse);
            return ErrorCode == 0;
        }

        /// <summary>
        /// 单轴相对运动指令
        /// </summary>
        /// <param name="axisIndex">轴号</param>
        /// <param name="distance">运动距离</param>
        /// <returns>操作结果。若操作成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</returns>
        public bool Move(int axisIndex, float distance)
        {
            ErrorCode = zmcaux.ZAux_Direct_Single_Move(ZMotionCardHandle, axisIndex, distance);
            return ErrorCode == 0;
        }

        /// <summary>
        /// 相对直线插补运动指令
        /// </summary>
        /// <param name="axisList">轴列表</param>
        /// <param name="distanceList">运动距离列表</param>
        /// <returns>操作结果。若操作成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</returns>
        public bool Move(int[] axisList, float[] distanceList) //重写Move函数,接收轴列表和轴距离列别作为参数，对应相等
        {
            if (axisList is null)
            {
                throw new ArgumentNullException(nameof(axisList));
            }

            if (distanceList is null)
            {
                throw new ArgumentNullException(nameof(distanceList));
            }

            if (axisList.Length != distanceList.Length)
            {
                ErrorMessage = "轴的数量和距离数量不一致";
                return false;
            }

            ErrorCode = zmcaux.ZAux_Direct_Move(ZMotionCardHandle, axisList.Length, axisList, distanceList);
            return ErrorCode == 0;
        }

        /// <summary>
        /// 单轴绝对运动指令
        /// </summary>
        /// <param name="axisIndex">轴号</param>
        /// <param name="position">绝对坐标</param>
        /// <returns>操作结果。若操作成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</returns>
        public bool MoveAbs(int axisIndex, float position)
        {
            //ZAux_Direct_Single_MoveAbs，单轴绝对运动，参数分别为连接句柄、轴号、绝对坐标
            ErrorCode = zmcaux.ZAux_Direct_Single_MoveAbs(ZMotionCardHandle, axisIndex, position);
            return ErrorCode == 0;
        }

        /// <summary>
        /// 绝对直线插补运动指令
        /// </summary>
        /// <param name="axisList">轴列表</param>
        /// <param name="positionList">绝对坐标列表</param>
        /// <returns>操作结果。若操作成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</returns>
        public bool MoveAbs(int[] axisList, float[] positionList)
        {
            if (axisList is null)
            {
                throw new ArgumentNullException(nameof(axisList));
            }

            if (positionList is null)
            {
                throw new ArgumentNullException(nameof(positionList));
            }

            if (axisList.Length != positionList.Length)
            {
               ErrorMessage = "轴的数量和坐标数量不一致";
                return false;
            }

            ErrorCode = zmcaux.ZAux_Direct_MoveAbs(ZMotionCardHandle, axisList.Length, axisList, positionList);
            return ErrorCode == 0;
        }

        /// <summary>
        /// 单轴连续运动指令
        /// 连续运动在切换方向时可以直接再次调用VMOVE直接替换前面的连续运动，不需要cancel后调用。
        /// </summary>
        /// <param name="axisIndex">轴号</param>
        /// <param name="position">绝对坐标</param>
        /// <returns>操作结果。若操作成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</returns>
        public bool VMove(int axisIndex, MotionDirection dir)
        {
           ErrorCode = zmcaux.ZAux_Direct_Single_Vmove(ZMotionCardHandle, axisIndex, (int)dir);
            return ErrorCode == 0;
        }

        /// <summary>
        /// 单轴停止运动（如果轴参与插补，也停止插补运动）
        /// 如果指定轴在BASE轴列表中，无论CANCEL主轴或者BASE轴列表中的轴，都停止轴组的插补运动
        /// MODE0~2减速度按FASTDEC和DECEL中最大的值。
        /// CANCEL后要调用绝对位置运动，必须先WAIT IDLE等待停止完成。
        /// </summary>
        /// <param name="axisIndex">轴号</param>
        /// <param name="mode">取消模式</param>
        /// <returns>操作结果。若操作成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</returns>
        public bool Cancel(int axisIndex, MotionCancelMode mode)
        {
            ErrorCode = zmcaux.ZAux_Direct_Single_Cancel(ZMotionCardHandle, axisIndex, (int)mode);
            return ErrorCode == 0;
        }

        #endregion 点位运动和直线插补

        #region 回零运动

        /// <summary>
        /// 单轴找原点运动指令
        /// </summary>
        /// <param name="axisIndex">轴号</param>
        /// <param name="mode">找原点模式</param>
        /// <returns>操作结果。若操作成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</returns>
        public bool Home(int axisIndex, HomeMode mode)
        {
            ErrorCode = zmcaux.ZAux_Direct_Single_Datum(ZMotionCardHandle, axisIndex, (int)mode);
            return ErrorCode == 0;
        }

        /// <summary>
        /// 设置轴找原点爬行速度
        /// </summary>
        /// <param name="axisIndex">轴号</param>
        /// <param name="val">爬行速度</param>
        /// <returns>操作结果。若操作成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</returns>
        public bool SetAxisCreepSpeed(int axisIndex, float val)
        {
            ErrorCode = zmcaux.ZAux_Direct_SetCreep(ZMotionCardHandle, axisIndex, val);
            return ErrorCode == 0;
        }

        /// <summary>
        /// 获取轴找原点爬行速度
        /// </summary>
        /// <param name="axisIndex">轴号</param>
        /// <param name="val">爬行速度</param>
        /// <returns>操作结果。若操作成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</returns>
        public bool GetAxisCreepSpeed(int axisIndex, out float val)
        {
            val = float.NaN;
            ErrorCode = zmcaux.ZAux_Direct_GetCreep(ZMotionCardHandle, axisIndex, ref val);
            return ErrorCode == 0;
        }

        /// <summary>
        /// 设置轴回零反找等待时间
        /// </summary>
        /// <param name="axisIndex">轴号</param>
        /// <param name="val">轴回零反找等待时间</param>
        /// <returns>操作结果。若操作成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</returns>
        public bool SetAxisHomeWait(int axisIndex, int val)
        {
            ErrorCode = zmcaux.ZAux_Direct_SetHomeWait(ZMotionCardHandle, axisIndex, val);
            return ErrorCode == 0;
        }

        /// <summary>
        /// 获取轴回零反找等待时间
        /// </summary>
        /// <param name="axisIndex">轴号</param>
        /// <param name="val">轴回零反找等待时间</param>
        /// <returns>操作结果。若操作成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</returns>
        public bool GetAxisHomeWait(int axisIndex, out int val)
        {
            val = int.MinValue;
            ErrorCode = zmcaux.ZAux_Direct_GetHomeWait(ZMotionCardHandle, axisIndex, ref val);
            return ErrorCode == 0;
        }

        #endregion 回零运动
        #endregion 基本运动控制

        #region 硬件接口访问与配置

        #region 数字输入输出

        /// <summary>
        /// 获取单个输入口的状态
        /// </summary>
        /// <param name="ioPortIndex">输入口编号</param>
        /// <param name="iOState">输入口状态</param>
        /// <returns>操作结果。若操作成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</returns>
        public bool GetIOIn(int ioPortIndex, out IoState iOState)
        {
            iOState = IoState.Unknown;
            uint state = uint.MinValue;
            //ZAux_Direct_GetIn，获取单个输入口状态，参数分别为连接句柄、输入口编号、返回输入口状态
           ErrorCode = zmcaux.ZAux_Direct_GetIn(ZMotionCardHandle, ioPortIndex, ref state);

            if (ErrorCode == 0)
            {
                try
                {
                    iOState = (IoState)state;
                    return true;
                }
                catch (Exception ex)
                {
                    ErrorMessage = $"获取输入口状态时出现意外值{ex.Message}";
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// 设置单个输出口的状态
        /// </summary>
        /// <param name="ioPortIndex">输出口编号</param>
        /// <param name="iOState">输出口状态</param>
        /// <returns>操作结果。若操作成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</returns>
        public bool SetIoOut(int ioPortIndex, IoState iOState)
        {
            if (iOState == IoState.Unknown)
            {
                ErrorMessage = "输出口的状态不能设置为\"Unknown\"";
                return false;
            }
            ErrorCode = zmcaux.ZAux_Direct_SetOp(ZMotionCardHandle, ioPortIndex, (uint)iOState);
            return ErrorCode == 0;
        }

        /// <summary>
        /// 设置多个输出口的状态
        /// </summary>
        /// <param name="ioPortStartIndex">输出口起始编号</param>
        /// <param name="ioPortEndIndex">输出口结束编号</param>
        /// <param name="states">输出口状态，按位设置</param>
        /// <returns>操作结果。若操作成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</returns>
        public bool SetIoOut(int ioPortStartIndex, int ioPortEndIndex, uint[] states)
        {
            ErrorCode = zmcaux.ZAux_Direct_SetOutMulti(ZMotionCardHandle, (ushort)ioPortStartIndex, (ushort)ioPortEndIndex, states);
            return ErrorCode == 0;
        }

        /// <summary>
        /// 获取单个输出口的状态
        /// </summary>
        /// <param name="ioPortIndex">输出口编号</param>
        /// <param name="iOState">输出口状态</param>
        /// <returns>操作结果。若操作成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</returns>
        public bool GetIOOut(int ioPortIndex, out IoState iOState)
        {
            iOState = IoState.Unknown;
            uint state = uint.MinValue;
           ErrorCode = zmcaux.ZAux_Direct_GetOp(ZMotionCardHandle, ioPortIndex, ref state);

            if (ErrorCode == 0)
            {
                try
                {
                    iOState = (IoState)state;
                    return true;
                }
                catch (Exception ex)
                {
                    ErrorMessage = $"获取输出口状态时出现意外值{ex.Message}";
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// 设置输入口信号反转状态
        /// </summary>
        /// <param name="ioPortIndex">输入口编号</param>
        /// <param name="invertState">反转状态</param>
        /// <returns>操作结果。若操作成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</returns>
        public bool SetInPortInvertState(int ioPortIndex, IoInvertedState invertState)
        {
            ErrorCode = zmcaux.ZAux_Direct_SetInvertIn(ZMotionCardHandle, ioPortIndex, (int)invertState);
            return ErrorCode == 0;
        }

        /// <summary>
        /// 获取输入口信号反转状态
        /// </summary>
        /// <param name="ioPortIndex">输入口编号</param>
        /// <param name="invertState">反转状态</param>
        /// <returns>操作结果。若操作成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</returns>
        public bool GetInPortInvertState(int ioPortIndex, out IoInvertedState invertState)
        {
            invertState = IoInvertedState.Unknown;
            var val = int.MinValue;
            ErrorCode = zmcaux.ZAux_Direct_GetInvertIn(ZMotionCardHandle, ioPortIndex, ref val);
            if (ErrorCode == 0)
            {
                invertState = (IoInvertedState)val;
            }
            return ErrorCode == 0;
        }

        #endregion 数字输入输出

        #region 模拟量输入输出

        /// <summary>
        /// 读取指定通道的单个模拟量输入口的值。（返回AD转换模块的刻度值）
        /// </summary>
        /// <param name="ioIndex">模拟输入口编号</param>
        /// <param name="val">模拟量刻度值</param>
        /// <returns>操作结果。若操作成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</returns>
        public bool GetADIn(int ioIndex, out float val)
        {
            val = float.MaxValue;
            ErrorCode = zmcaux.ZAux_Direct_GetAD(ZMotionCardHandle, ioIndex, ref val);
            return ErrorCode == 0;
        }

        /// <summary>
        /// 设置指定通道的单个模拟量输出口的值
        /// </summary>
        /// <param name="ioIndex">模拟输出口编号</param>
        /// <param name="val">设置值</param>
        /// <returns>操作结果。若操作成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</returns>
        public bool SetDAOut(int ioIndex, float val)
        {
            ErrorCode = zmcaux.ZAux_Direct_SetDA(ZMotionCardHandle, ioIndex, val);
            return ErrorCode == 0;
        }

        /// <summary>
        /// 读取指定通道的单个模拟量输出口的值
        /// </summary>
        /// <param name="ioIndex">模拟输入口编号</param>
        /// <param name="val">模拟量刻度值</param>
        /// <returns>操作结果。若操作成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</returns>
        public bool GetDAOut(int ioIndex, out float val)
        {
            val = float.MaxValue;
            ErrorCode = zmcaux.ZAux_Direct_GetDA(ZMotionCardHandle, ioIndex, ref val);
            return ErrorCode == 0;
        }

        #endregion 模拟量输入输出

 
        #region 编码器
        /// <summary>
        /// 获取轴的内部编码器的值(总线绝对值伺服时为绝对值位置)
        /// </summary>
        /// <param name="axisIndex">轴号</param>
        /// <param name="val">编码器的值</param>
        /// <returns>操作结果。若操作成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</returns>
        public bool GetAxisEncoder(int axisIndex, out float val)
        {
            val = float.NaN;
            ErrorCode = zmcaux.ZAux_Direct_GetEncoder(ZMotionCardHandle, axisIndex, ref val);
            return ErrorCode == 0;
        }

        /// <summary>
        /// 获取轴的反馈速度
        /// </summary>
        /// <param name="axisIndex">轴号</param>
        /// <param name="val">轴的反馈速度</param>
        /// <returns>操作结果。若操作成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</returns>
        public bool GetAxisMSpeed(int axisIndex, out float val)
        {
            val = float.NaN;
           ErrorCode = zmcaux.ZAux_Direct_GetMspeed(ZMotionCardHandle, axisIndex, ref val);
            return ErrorCode == 0;
        }
        #endregion 编码器

        #region CAN扩展配置

        /// <summary>
        /// 设置轴地址
        /// </summary>
        /// <param name="axisIndex">轴号</param>
        /// <param name="addr">地址{
        /// [ZCAN扩展轴时：addr=扩展板拨码ID+(32 * 扩展板的轴编号)]
        /// [总线驱动器映射轴号时：addr=(槽位号"左移"16位)+驱动器编号+1]
        /// [本地脉冲轴号重映射时：addr=(-1"左移"16位)+要修改的本地脉冲轴号]
        /// }</param>
        /// <returns>操作结果。若操作成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</returns>
        public bool SetAxisAddress(int axisIndex, int addr)
        {
            ErrorCode = zmcaux.ZAux_Direct_SetAxisAddress(ZMotionCardHandle, axisIndex, addr);
            return ErrorCode == 0;
        }

        /// <summary>
        /// 获取轴地址
        /// </summary>
        /// <param name="axisIndex">轴号</param>
        /// <param name="addr">地址</param>
        /// <returns>操作结果。若操作成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</returns>
        public bool GetAxisAddress(int axisIndex, out int addr)
        {
            addr = int.MinValue;
            ErrorCode = zmcaux.ZAux_Direct_GetAxisAddress(ZMotionCardHandle, axisIndex, ref addr);
            return ErrorCode == 0;
        }

        #endregion CAN扩展配置
        #endregion 硬件接口访问与配置


        #region 板卡数据交互

        #region MODBUS、TABLE、VR

        /// <summary>
        /// 将数据写入Table
        /// </summary>
        /// <param name="startIndex">写入Table的起始索引</param>
        /// <param name="val">写入的数据</param>
        /// <returns>操作结果。若操作成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</returns>
        public bool WriteTable(int startIndex, float[] val)
        {
            ErrorCode = zmcaux.ZAux_Direct_SetTable(ZMotionCardHandle, startIndex, val.Length, val);
            return ErrorCode == 0;
        }

        /// <summary>
        /// 读取Table中的数据
        /// </summary>
        /// <param name="startIndex">读取table的起始索引</param>
        /// <param name="length">读取的长度</param>
        /// <param name="val">读取的数据</param>
        /// <returns>操作结果。若操作成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</returns>
        public bool ReadTable(int startIndex, int length, out float[] val)
        {
            val = new float[length];
            ErrorCode = zmcaux.ZAux_Direct_GetTable(ZMotionCardHandle, startIndex, length, val);
            return ErrorCode == 0;
        }
        /// <summary>
        /// 设置MODBUS位寄存器
        /// </summary>
        /// <param name="start">modbus_bit起始地址编号</param>
        /// <param name="inum">写的个数</param>
        /// <param name="pada">数据列表</param>
        /// <returns></returns>
        public bool SetModbus_Set0x(ushort start, ushort inum, byte[] pada)
        {
            ErrorCode =zmcaux.ZAux_Modbus_Set0x(ZMotionCardHandle, start, inum, pada);
            return ErrorCode == 0;
        }

        #endregion MODBUS、TABLE、VR

        #endregion 板卡数据交互

        #region EtherCat与Rtex总线

        #region 总线初始化

        /// <summary>
        /// 总线初始化
        /// </summary>
        /// <returns>操作结果。若操作成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</returns>
        public bool BusCmd_Init()
        {
            ErrorCode = zmcaux.ZAux_BusCmd_InitBus(ZMotionCardHandle);
            return ErrorCode == 0;
        }

        /// <summary>
        /// 获取总线初始化状态
        /// </summary>
        /// <param name="isSuccees">状态标志。若初始化成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</param>
        /// <returns>操作结果。若操作成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</returns>
        public bool BusCmd_GetInitIsSucceed(out bool isSuccees)
        {
            isSuccees = false;
            int rt = int.MinValue;
            ErrorCode = zmcaux.ZAux_BusCmd_GetInitStatus(ZMotionCardHandle, ref rt);
            if (ErrorCode == 0 && rt == 1)
            {
                isSuccees = true;
            }
            return ErrorCode == 0;
        }

        /// <summary>
        /// 获取轴的错误标志
        /// 和 AXISSTATUS 做与运算来决定那些错误需要关闭 WDOG。
        /// </summary>
        /// <param name="axisIndex">轴号</param>
        /// <param name="mark">轴的错误标志代码</param>
        /// <returns>操作结果。若操作成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</returns>
        public bool BusCmd_GetErrorMark(int axisIndex, out int mark)
        {
            mark = int.MinValue;
            ErrorCode = zmcaux.ZAux_Direct_GetErrormask(ZMotionCardHandle, axisIndex, ref mark);
            return ErrorCode == 0;
        }

        /// <summary>
        /// 设置轴的错误标志
        /// </summary>
        /// <param name="axisIndex">轴号</param>
        /// <param name="mark">设置值</param>
        /// <returns>操作结果。若操作成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</returns>
        public bool BusCmd_SetErrorMark(int axisIndex, int mark)
        {
            ErrorCode = zmcaux.ZAux_Direct_SetErrormask(ZMotionCardHandle, axisIndex, mark);
            return ErrorCode == 0;
        }

        /// <summary>
        /// 获取节点总线状态
        /// </summary>
        /// <param name="slotIndex">槽位号</param>
        /// <param name="nodeIndex">节点号</param>
        /// <param name="nodeStatus">节点状态代码（按位处理 bit0-节点是否存在 bit1-通讯状态 bit2-节点状态）</param>
        /// <returns>操作结果。若操作成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</returns>
        public bool BusCmd_GetNodeStatus(uint slotIndex, uint nodeIndex, out uint nodeStatus)
        {
            nodeStatus = uint.MinValue;
            ErrorCode = zmcaux.ZAux_BusCmd_GetNodeStatus(ZMotionCardHandle, slotIndex, nodeIndex, ref nodeStatus);
            return ErrorCode == 0;
        }

        #endregion 总线初始化

        #region 总线驱动器运动调用

        /// <summary>
        /// 设置总线轴使能状态
        /// </summary>
        /// <param name="axisIndex">轴号</param>
        /// <param name="enabled">使能状态</param>
        /// <returns>操作结果。若操作成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</returns>
        public bool BusCmd_SetAxisEnable(int axisIndex, bool enabled)
        {
            ErrorCode = zmcaux.ZAux_Direct_SetAxisEnable(ZMotionCardHandle, axisIndex, enabled ? 1 : 0);
            return ErrorCode == 0;
        }

        /// <summary>
        /// 获取总线轴使能状态
        /// </summary>
        /// <param name="axisIndex">轴号</param>
        /// <param name="enabled">使能状态</param>
        /// <returns>操作结果。若操作成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</returns>
        public bool BusCmd_GetAxisEnabled(int axisIndex, out bool enabled)
        {
            enabled = false;
            int rt = int.MinValue;
            ErrorCode = zmcaux.ZAux_Direct_GetAxisEnable(ZMotionCardHandle, axisIndex, ref rt);
            if (ErrorCode == 0)
            {
                enabled = rt == 1;
            }
            return ErrorCode == 0;
        }

        #endregion 总线驱动器运动调用

        #region 报警的清除与信息获取

        /// <summary>
        /// 清除总线伺服报警
        /// </summary>
        /// <param name="axisIndex">轴号</param>
        /// <param name="mode">清除模式,0清楚当前，1清除历史，2清除外部输入警告</param>
        /// <returns>操作结果。若操作成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</returns>
        public bool BusCmd_ClearDriveError(int axisIndex, ServoErrorClearMode mode)
        {
            ErrorCode = zmcaux.ZAux_BusCmd_DriveClear(ZMotionCardHandle, (uint)axisIndex, (uint)mode);
            return ErrorCode == 0;
        }

        /// <summary>
        /// 获取节点数量
        /// </summary>
        /// <param name="slotIndex">槽位号</param>
        /// <param name="val">节点数量</param>
        /// <returns>操作结果。若操作成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</returns>
        public bool BusCmd_GetNodeNum(int slotIndex, out int val)
        {
            val = int.MinValue;
            ErrorCode = zmcaux.ZAux_BusCmd_GetNodeNum(ZMotionCardHandle, slotIndex, ref val);
            return ErrorCode == 0;
        }

        /// <summary>
        /// 获取节点信息
        /// </summary>
        /// <param name="slotIndex">槽位号</param>
        /// <param name="nodeIndex">节点号</param>
        /// <param name="infoIndex">信息编号（0:厂商编号 1:设备编号 2:设备版本 3:别名 10:IN个数 11: OUT个数）</param>
        /// <param name="info">节点信息</param>
        /// <returns>操作结果。若操作成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</returns>
        public bool BusCmd_GetNodeInfo(int slotIndex, int nodeIndex, BusNodeInfoIndex infoIndex, out int info)
        {
            info = int.MinValue;
           ErrorCode = zmcaux.ZAux_BusCmd_GetNodeInfo(ZMotionCardHandle, (uint)slotIndex, (uint)nodeIndex, (uint)infoIndex, ref info);
            return ErrorCode == 0;
        }

        #endregion 报警的清除与信息获取

        #region SDO
        //SDO（Service Data Object）： 
        //    SDO是CANopen网络中用于配置和管理节点参数的一种对象类型。它通过请求-响应机制实现数据的读取和写入。
        //    SDO适用于配置节点参数、读取设备状态和进行故障诊断等场景。
        //    SDO的数据传输是基于请求和响应的，需要节点之间进行交互。
        /// <summary>
        /// 通过槽位号和节点号进行SDO写入
        /// </summary>
        /// <param name="slotIndex">槽位号</param>
        /// <param name="nodeIndex">节点号</param>
        /// <param name="dctIndex">对象字典编号</param>
        /// <param name="subIndex">对象字典子编号</param>
        /// <param name="type">数据类型</param>
        /// <param name="val">写入的值</param>
        /// <returns>操作结果。若操作成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</returns>
        public bool BusCmd_SDOWrite(int slotIndex, int nodeIndex, uint dctIndex, uint subIndex, BusSdoDataType type, int val)
        {
            ErrorCode = zmcaux.ZAux_BusCmd_SDOWrite(ZMotionCardHandle, (uint)slotIndex, (uint)nodeIndex, dctIndex, subIndex, (uint)type, val);
            return ErrorCode == 0;
        }

        /// <summary>
        /// 通过槽位号和节点号进行SDO读取
        /// </summary>
        /// <param name="slotIndex">槽位号</param>
        /// <param name="nodeIndex">节点号</param>
        /// <param name="dctIndex">对象字典编号</param>
        /// <param name="subIndex">对象字典子编号</param>
        /// <param name="type">数据类型</param>
        /// <param name="val">写入的值</param>
        /// <returns>操作结果。若操作成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</returns>
        public bool BusCmd_SDORead(int slotIndex, int nodeIndex, uint dctIndex, uint subIndex, BusSdoDataType type, out int val)
        {
            val = int.MinValue;
            ErrorCode = zmcaux.ZAux_BusCmd_SDORead(ZMotionCardHandle, (uint)slotIndex, (uint)nodeIndex, dctIndex, subIndex, (uint)type, ref val);
            return ErrorCode == 0;
        }

        #endregion SDO
        #endregion EtherCat与Rtex总线

        #region 位置比较输出

        /// <summary>
        /// 软件位置比较输出
        /// 只有ATYPE为4时才是比较反馈位置(MPOS)，默认出厂的ATYPE为1或7比较的是命令位置(DPOS)。
        /// </summary>
        /// <param name="axisIndex">轴号</param>
        /// <param name="comparatorIndex">比较器编号</param>
        /// <param name="outPortIndex">输出口索引</param>
        /// <param name="setPos">比较输出的起始位置</param>
        /// <param name="resetPos">比较输出的结束位置</param>
        /// <param name="enable">比较器使能</param>
        /// <param name="outState">输出状态</param>
        /// <returns>操作结果。若操作成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</returns>
        public bool SoftwarePosCompare(int axisIndex, int comparatorIndex, int outPortIndex, float setPos, float resetPos, bool enable = true, IoState outState = IoState.On)
        {
            ErrorCode = zmcaux.ZAux_Direct_Pswitch(ZMotionCardHandle, comparatorIndex, enable ? 1 : 0, axisIndex, outPortIndex, (int)outState, setPos, resetPos);
            return ErrorCode == 0;
        }

        /// <summary>
        /// 脉冲轴硬件位置比较输出（4 系列产品脉冲轴与编码器轴支持）。
        /// 每个比较点触发都会使得当前输出口电平翻转。
        /// 只有ATYPE为4时才是比较反馈位置(MPOS)，默认出厂的ATYPE为1或7比较的是命令位置(DPOS)。
        /// 没有比较完所有点的话，一定要设置mode值为2，通过HW_PSWITCH(2)指令停止并删除没有完成的比较点，否则后面此输出通道会工作不正常。
        /// </summary>
        /// <param name="axisIndex">轴号</param>
        /// <param name="tableStartIndex">Table起始位置索引</param>
        /// <param name="tableEndIndex">Table结束位置索引</param>
        /// <param name="mode">比较器操作模式</param>
        /// <param name="direction">比较方向</param>
        /// <returns>操作结果。若操作成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</returns>
        public bool HardwardPosCompare(int axisIndex, int tableStartIndex, int tableEndIndex, ComparatorMode mode = ComparatorMode.Start, CompareDirection direction = CompareDirection.Forward)
        {
            ErrorCode = zmcaux.ZAux_Direct_HwPswitch(ZMotionCardHandle, axisIndex, (int)mode, (int)direction, 0, tableStartIndex, tableEndIndex);
            return ErrorCode == 0;
        }

        #endregion 位置比较输出

        #region 调试工具

        /// <summary>
        /// 示波器触发
        /// </summary>
        /// <returns>操作结果。若操作成功，则为<see cref="true"/>；否则，为<see cref="false"/>。</returns>
        public bool Trigger()
        {
            ErrorCode =zmcaux.ZAux_Trigger(ZMotionCardHandle);
            return ErrorCode == 0;
        }

        #endregion 调试工具
        /// <summary>
        /// 上位机调用上位机未封装的 Basic 指令功能
        /// </summary>
        /// <param name="Command">发送的命令字符串</param>
        /// <param name="rtResult">返回的字符串结果</param>
        /// <returns></returns>
        public bool SendCommand(string Command, out string rtResult)
        {
            uint length = 1024;
            StringBuilder rtsb = new StringBuilder((int)length);
            ErrorCode = zmcaux.ZAux_Execute(ZMotionCardHandle, Command, rtsb, length);
            string tmp = rtsb.ToString();
            rtResult = tmp.Length > 0 ? tmp.Substring(0, tmp.Length - 1) : tmp;
            return ErrorCode == 0;
        }
        public bool Execute(string command, StringBuilder str)
        {
            ErrorCode = zmcaux.ZAux_Execute(ZMotionCardHandle, command, str, 1000);
            return ErrorCode == 0;
        }
        /// <summary>
        /// 执行指令
        /// </summary>
        /// <param name="processName"></param>
        /// <param name="rt"></param>
        /// <returns></returns>
        public bool ExecuteSubProcess(string processName, ref StringBuilder rt)
        {
            return zmcaux.ZAux_Execute(ZMotionCardHandle, $@"STOPTASK 5", new StringBuilder(),
                       uint.MaxValue) == 0 &&
                   zmcaux.ZAux_Execute(ZMotionCardHandle, $@"RUNTASK 5,{processName}", new StringBuilder(),
                       uint.MaxValue) == 0;
        }

        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            SendCommand($"CANCEL(2)", out string _);
        }

        public void SetAxisInfo(Axis axis)
        {
           // ZmotionFun.zmotionFun.SetAxisUnits(axis.index, axis.axisUnits);
          ZmotionFun.zmotionFun.SetAxisSpeed(axis.index, axis.dSpeed);
           // ZmotionFun.zmotionFun.SetAxisAccelSpeed(axis.index,0.2f);
           // ZmotionFun.zmotionFun.SetAxisDecelSpeed(axis.index, 0.2f);
           // ZmotionFun.zmotionFun.SetAxisFastDecel(axis.index, axis.fastDeccelSpeed);
           // ZmotionFun.zmotionFun.SetAxisCreepSpeed(axis.index, axis.creepSpeed);
           // ZmotionFun.zmotionFun.SetAxisHomeWait(axis.index, axis.homeWait);           
           // ZmotionFun.zmotionFun.SetAxisMaxSpeed(axis.index, axis.maxSpeed);
           // ZmotionFun.zmotionFun.SetAxisHomeIOIn(axis.index, axis.homeIOIn);
           // ZmotionFun.zmotionFun.SetAxisForWardLimitIOIn(axis.index, axis.forLimitIOIn);
           // ZmotionFun.zmotionFun.SetAxisBackwardLimitIOIn(axis.index, axis.backLimitIOIn);           
           // ZmotionFun.zmotionFun.SetInPortInvertState(axis.forLimitIOIn, axis.ioInvertedState);
           // ZmotionFun.zmotionFun.SetInPortInvertState(axis.backLimitIOIn, axis.ioInvertedState);
           // ZmotionFun.zmotionFun.SetIoOut(axis.enabledIOOut, axis.isEnabled ? IoState.On : IoState.Off);
        }

        #endregion 方法
    }
}

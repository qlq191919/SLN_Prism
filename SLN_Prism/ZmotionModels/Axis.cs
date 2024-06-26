﻿using ImTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SLN_Prism.ZmotionModels
{
   
    public class Axis
    {
        /// <summary>
        /// 轴索引
        /// </summary>
        public int index { get; set; }
        /// <summary>
        /// 轴名称
        /// </summary>
        public string axisName { get; set; }
        /// <summary>
        /// 轴类型
        /// </summary>
        public AxisType axisType { get; set; }
        /// <summary>
        /// 轴状态，
        /// </summary>
        public int axisStatus { get; set; }
        /// <summary>
        /// 清除报警信息
        /// </summary>
        public int clearAlarmIOOut { get; set; }
        /// <summary>
        /// 回原模式
        /// </summary>
        public HomeMode homeType { get; set; }
        /// <summary>
        ///是否空闲中
        /// </summary>
        public bool isIdle { get; set; }
        /// <summary>
        /// 轴使能
        /// </summary>
        public int enabledIOOut { get; set; }
        public bool isEnabled { get; set; }

        /// <summary>
        /// 轴原点
        /// </summary>
        public int homeIOIn { get; set; }
        /// <summary>
        /// 正限位
        /// </summary>
        public int forLimitIOIn { get; set; }
        /// <summary>
        /// 负限位
        /// </summary>
        public int backLimitIOIn { get; set; }

        /// <summary>
        /// 脉冲当量，假设电机 U=3600 脉冲转一圈，丝杠一圈螺距 P=2mm：UNITS=U/360=3600/360=10；//此时 MOVE(1)，电机转 1°
        ///UNITS=U/P=3600/2=1800，//此时 MOVE(1)，工作台走 1mm。机台存在减速比时，要把减速比（i）也算上，UNITS=U* i/P=3600*2/2=3600
        /// </summary>
        public int axisUnits { get; set; }
        
        /// <summary>
        /// 命令位置
        /// </summary>
        public float dPosition { get; set; }
        /// <summary>
        /// 反馈位置
        /// </summary>
        public double mPosition { get; set; }
        /// <summary>
        /// 命令速度
        /// </summary>
        public float dSpeed {get; set; }
        /// <summary>
        /// 反馈速度
        /// </summary>
        public double mSpeed { get; set; }
        /// <summary>
        /// 加速度
        /// </summary>
        public double accelSpeed { get; set; }
        /// <summary>
        /// 减速度
        /// </summary>
        public double decelSpeed { get; set; }
        /// <summary>
        /// 最大速度
        /// </summary>
        public int maxSpeed { get; set; }
        /// <summary>
        /// 快速加速度
        /// </summary>
        public float fastDeccelSpeed { get; set; }
        /// <summary>
        /// 回原爬行速度
        /// </summary>
        public float creepSpeed { get; set; }
        /// <summary>
        /// 回原等待时间
        /// </summary>
        public int homeWait { get; set; }
        /// <summary>
        /// 运动缓冲
        /// </summary>
        public bool axisLoadedStatus { get; set; }
        public IoInvertedState ioInvertedState { get; set; }

       public Axis(int index, string axisName=null)
        {
            this.index = index;
            this.axisName = axisName;
            
        }
        
        

        
    }
}

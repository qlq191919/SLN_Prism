using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLN_Prism.Zmotion
{

    public enum AxisType
    {
        虚拟轴 = 0,
        脉冲方向方式的步进或伺服 = 1,
        模拟信号控制方式的伺服 = 2,
        正交编码器 = 3,
        脉冲方向输出正交编码器输入 = 4,
        脉冲方向输出脉冲方向编码器输入 = 5,
        脉冲方向方式的编码器 = 6,
        脉冲方向方式步进或伺服AndEZ信号输入 = 7,
        ZCAN扩展脉冲方向方式步进或伺服 = 8,
        ZCAN扩展正交编码器 = 9,
        ZCAN扩展脉冲方向方式的编码器 = 10,
        振镜轴 = 21,

        [Description("需Rtex控制器")] RTEX周期位置模式 = 50,

        [Description("需Rtex控制器")] RTEX周期速度模式 = 51,

        [Description("需Rtex控制器,请先关闭驱动器2自由度控制模式，并设置设置速度限制")]
        RTEX周期力矩模式 = 52,

        [Description("需支持EtherCAT")] ECAT周期位置模式 = 65,

        [Description("需支持EtherCAT,Profile要设置为20或以上")]
        ECAT周期速度模式 = 66,

        [Description("需支持EtherCAT,PROFILE要设置为30或以上")]
        ECAT周期力矩模式 = 67,

        [Description("只读取编码器，需支持EtherCAT")] ECAT自定义操作 = 70
    }

    /// <summary>
    /// 轴运动方向
    /// </summary>
    public enum MotionDirection
    {
        /// <summary>
        /// 正转
        /// </summary>
        [Description("轴正向运动")] Forward = 1,

        /// <summary>
        /// 反转
        /// </summary>
        [Description("轴负向运动")] Backward = -1
    }

    /// <summary>
    /// 运动取消的模式
    /// </summary>
    public enum MotionCancelMode
    {
        /// <summary>
        /// 取消当前运动
        /// </summary>
        [Description("取消当前运动")] Current = 0,

        /// <summary>
        /// 取消缓冲的运动
        /// </summary>
        [Description("取消缓冲的运动")] Buffered = 1,

        /// <summary>
        /// 取消当前运动和缓冲运动
        /// </summary>
        [Description("取消当前运动和缓冲运动")] CurrentAndBuffered = 2,

        /// <summary>
        /// 立即中断脉冲发送
        /// </summary>
        [Description("立即中断脉冲发送")] InterruptPulse = 3
    }

    /// <summary>
    /// 轴找原点模式
    /// </summary>
    public enum HomeMode
    {
        [Description("清除所有轴的错误状态")] ClearAxisError = 0,

        /// <summary>
        /// 轴以CREEP速度正向运行直到Z信号出现。碰到限位开关会直接停止。DPOS值重置为0,同时纠正MPOS。
        /// </summary>
        [Description("轴以CREEP速度正向运行直到Z信号出现。碰到限位开关会直接停止。DPOS值重置为0，同时纠正MPOS。")]
        ForwardUntilZSignal = 1,

        /// <summary>
        /// 轴以CREEP速度反向运行直到Z信号出现。碰到限位开关会直接停止。DPOS值重置为0，同时纠正MPOS。
        /// </summary>
        [Description("轴以CREEP速度反向运行直到Z信号出现。碰到限位开关会直接停止。DPOS值重置为0，同时纠正MPOS。")]
        BackwardUntilZSignal = 2,

        /// <summary>
        /// 轴以SPEED速度正向运行，直到碰到原点开关。然后轴以CREEP速度反向运动直到离开原点开关。
        /// 找原点阶段碰到正限位开关会直接停止。爬行阶段碰到负限位开关会直接停止。DPOS值重置为0，同时纠正MPOS。
        /// </summary>
        [Description(
            "轴以SPEED速度正向运行，直到碰到原点开关。然后轴以CREEP速度反向运动直到离开原点开关。找原点阶段碰到正限位开关会直接停止。爬行阶段碰到负限位开关会直接停止。DPOS值重置为0，同时纠正MPOS。")]
        ForwardUnitlHomeAndBackwardLeave = 3,

        /// <summary>
        /// 轴以SPEED速度反向运行，直到碰到原点开关。然后轴以CREEP速度正向运动直到离开原点开关。
        /// 找原点阶段碰到负限位开关会直接停止。爬行阶段碰到正限位开关会直接停止。DPOS值重置为0，同时纠正MPOS。
        /// </summary>
        [Description(
            "轴以SPEED速度反向运行，直到碰到原点开关。然后轴以CREEP速度正向运动直到离开原点开关。找原点阶段碰到负限位开关会直接停止。爬行阶段碰到正限位开关会直接停止。DPOS值重置为0，同时纠正MPOS。")]
        BackwardUntilHomeAndForwardLeave = 4,

        /// <summary>
        /// 轴以SPEED速度正向运行，直到碰到原点开关。然后轴以CREEP速度反向运动直到离开原点开关，然后再继续以爬行速度反转直到碰到Z信号。
        /// 碰到限位开关会直接停止。DPOS值重置为0，同时纠正MPOS。
        /// </summary>
        [Description(
            "轴以SPEED速度正向运行，直到碰到原点开关。然后轴以CREEP速度反向运动直到离开原点开关，然后再继续以爬行速度反转直到碰到Z信号。 碰到限位开关会直接停止。DPOS值重置为0，同时纠正MPOS。")]
        ForwardUnitlHomeAndBackwardLeaveFinallyBackwardUntilZSignal = 5,

        /// <summary>
        /// 轴以SPEED速度反向运行，直到碰到原点开关。然后轴以CREEP速度正向运动直到离开原点开关，然后再继续以爬行速度正转直到碰到Z信号。
        /// 碰到限位开关会直接停止。DPOS值重置为0，同时纠正MPOS。
        /// </summary>
        [Description(
            "轴以SPEED速度反向运行，直到碰到原点开关。然后轴以CREEP速度正向运动直到离开原点开关，然后再继续以爬行速度正转直到碰到Z信号。碰到限位开关会直接停止。DPOS值重置为0，同时纠正MPOS。")]
        BackwardUnitlHomeAndForwardLeaveFinallyForwardUntilZSignal = 6,

        /// <summary>
        /// 轴以SPEED速度正向运行，直到碰到原点开关。碰到限位开关会直接停止。
        /// </summary>
        [Description("轴以SPEED速度正向运行，直到碰到原点开关。碰到限位开关会直接停止。")]
        ForwardUntilHome = 8,

        /// <summary>
        /// 轴以SPEED速度反向运行，直到碰到原点开关。碰到限位开关会直接停止。
        /// </summary>
        [Description("轴以SPEED速度反向运行，直到碰到原点开关。碰到限位开关会直接停止。")]
        BackwardUntilHome = 9,

        /// <summary>
        /// 使用Ethercat驱动器回零功能，具体请参考正运动开发手册。
        /// </summary>
        [Description("使用Ethercat驱动器回零功能，具体请参考正运动开发手册。")]
        EtherCatDriverHome = 21,

        /// <summary>
        /// 加上此模式表示碰到限位后反找, 不会碰到限位停止。例如13=模式3+限位反找10，用于原点在正中间的情况。
        /// </summary>
        [Description("加上此模式表示碰到限位后反找, 不会碰到限位停止。例如13=模式3+限位反找10，用于原点在正中间的情况。")]
        WhenLimitedAfterBackwardLookup = 10,

        /// <summary>
        /// ATYPE=4时，加上此模式表示接入编码器后可以自动清零MPOS(仅限4系列)
        /// </summary>
        [Description("ATYPE=4时，加上此模式表示接入编码器后可以自动清零MPOS(仅限4系列)")]
        AutoResetMPos = 100,


        Mode14 = 14,

        Mode13 = 13
    }

    /// <summary>
    /// IO状态
    /// </summary>
    public enum IoState
    {
        /// <summary>
        /// 未知状态
        /// </summary>
        [Description("未知")] Unknown = -1,

        /// <summary>
        /// Off状态
        /// </summary>
        [Description("Off")] Off = 0,

        /// <summary>
        /// On状态
        /// </summary>
        [Description("On")] On = 1
    }

    /// <summary>
    /// IO反转开关状态
    /// </summary>
    public enum IoInvertedState
    {
        /// <summary>
        /// 未知状态
        /// </summary>
        [Description("未知")] Unknown = -1,

        /// <summary>
        /// 反转关
        /// </summary>
        [Description("Off")] Off = 0,

        /// <summary>
        /// 反转开
        /// </summary>
        [Description("On")] On = 1
    }

    /// <summary>
    /// 总线伺服报警清除模式
    /// </summary>
    public enum ServoErrorClearMode
    {
        /// <summary>
        /// 清除当前报警
        /// </summary>
        [Description("清除当前报警")] Current = 0,

        /// <summary>
        /// 清除历史报警
        /// </summary>
        [Description("清除历史报警")] History = 1,

        /// <summary>
        /// 清除外部输入警告
        /// </summary>
        [Description("清除外部输入警告")] ExternalInputWarning = 2
    }

    /// <summary>
    /// 总线节点信息索引
    /// </summary>
    public enum BusNodeInfoIndex
    {
        /// <summary>
        /// 厂商代码
        /// </summary>
        [Description("厂商代码")] VenderCode = 0,

        /// <summary>
        /// 设备代码
        /// </summary>
        [Description("设备代码")] DeviceCode = 1,

        /// <summary>
        /// 设备版本
        /// </summary>
        [Description("设备版本")] DeviceVersion = 2,

        /// <summary>
        /// 设备别名
        /// </summary>
        [Description("设备别名")] DeviceAlias = 3,

        /// <summary>
        /// 数字输入口数量
        /// </summary>
        [Description("数字输入口数量")] DigitalInputPorts = 10,

        /// <summary>
        /// 数字输出口数量
        /// </summary>
        [Description("数字输出口数量")] DigitalOutputPorts = 11,

        /// <summary>
        /// 模拟量输入口数量
        /// </summary>
        [Description("模拟量输入口数量")] AnalogueInputPorts = 12,

        /// <summary>
        /// 模拟量输出口数量
        /// </summary>
        [Description("模拟量输出口数量")] AnalogueOutputPorts = 13
    }

    /// <summary>
    /// 总线SDO数据类型
    /// </summary>
    public enum BusSdoDataType
    {
        Boolean = 1,
        Integer8 = 2,
        Integer16 = 3,
        Integer32 = 4,
        UnsignedInteger8 = 5,
        UnsignedInteger16 = 6,
        UnsignedInteger32 = 7
    }

    /// <summary>
    /// 比较器模式
    /// </summary>
    public enum ComparatorMode
    {
        /// <summary>
        /// 启动比较器
        /// </summary>
        [Description("启动比较器")] Start = 1,

        /// <summary>
        /// 停止比较器并删除未完成的比较点
        /// </summary>
        [Description("停止比较器并删除未完成的比较点")] StopAndClearPos = 2
    }

    /// <summary>
    /// 比较模式
    /// </summary>
    public enum CompareDirection
    {
        /// <summary>
        /// 负向
        /// </summary>
        [Description("负向")] Backward = 0,

        /// <summary>
        /// 正向
        /// </summary>
        [Description("正向")] Forward = 1,

        /// <summary>
        /// 不判断方向
        /// </summary>
        [Description("不判断方向")] Ignore = 2
    }
    /// <summary>
    ///  1、3、8
    /// </summary>
    public enum AxisErrorStatus
    {
        随动误差超限告警 = 1,
        与远程轴通讯出错 = 2,
        远程驱动器报错 = 3,
        正向硬限位 = 4,
        反向硬限位 = 5,
        找原点中 = 64,
        HOLD速度保持信号输入 = 7,
        随动误差超限出错 = 8,
        超过正向软限位 = 9,
        超过负向软限位 = 10,
        CANCEL执行中 = 11,
        脉冲频率超过MAX_SPEED限制 = 12, // 需要修改降速或修改MAX_SPEED
        机械手指令坐标错误 = 14,
        电源异常 = 18,
        运动中触发特殊运动指令失败 = 21,
        告警信号输入 = 22,
        轴进入了暂停状态 = 23,
    }
}


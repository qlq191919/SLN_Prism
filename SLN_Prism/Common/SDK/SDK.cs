using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLN_Prism.Common.SDK
{
    public enum ProcedureControllerStatus
    {
        Running,
        Error,
        Standby,
        Reset,
        EmergencyStop,
        Full,
    }

    public enum AutoManualStatus
    {
        ManualMode,
        AutoMode,
    }

    /// <summary>
    /// 托盘仓状态
    /// </summary>
    public enum TrayHouseState
    {
        StandBy,
        Running,
        Error,
    }

    public enum AccessDoorState
    {
        Open,
        Close,
    }

    /// <summary>
    /// 主流程状态
    /// </summary>
    public enum MainState
    {
        Running,
        Error,
        StandBy,
    }

    public enum HardWareState
    {
        Running,
        Error,
        StandBy,
    }
}

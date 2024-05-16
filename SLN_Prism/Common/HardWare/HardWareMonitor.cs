using SLN_Prism.Common.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SLN_Prism.Common.HardWare
{
    public class HardWareMonitor
    {
        private CancellationTokenSource _token4Monitor;  //声明一个CancellationTokenSource对象，用于接收实例、取消监控任务
        private HardWareState _axesState;
        public static HardWareMonitor Instance { get; } = new HardWareMonitor(); //创建一个公开的实例，实现全局访问和状态共享

        public void StartHardWareMonitor()
        {
            Task.Run(() =>
            {
                _token4Monitor = new CancellationTokenSource();
                while (!_token4Monitor.IsCancellationRequested)
                {
                }
            });
        }

        public void StopHardWareMonitor()
        {
            _token4Monitor.Cancel();
        }

        public HardWareState AxesState
        {
            get => _axesState;
            set
            {
                if (_axesState != value)
                {
                    _axesState = value;
                    if (_axesState == HardWareState.Error)
                    {
                    }
                }
            }
        }
    }
}

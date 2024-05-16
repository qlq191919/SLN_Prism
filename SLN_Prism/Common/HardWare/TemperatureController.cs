using Modbus.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SLN_Prism.Common.HardWare
{
    //温控设备 ModbusTCP
    public class TemperatureController
    {
        public static TemperatureController Instance { get; private set; } = new TemperatureController();
        private TcpClient _client;
        private ModbusIpMaster _master;
        public event EventHandler<bool> RunningStatusChangedEvent;
        public event EventHandler<double[]> TemperatureChangedEvent;
        private bool _isRunning;

        private bool _connected = false;
        public bool Connected  // 连接状态
        {
            get { return _connected; }
            private set
            {
                if (_connected != value)
                {
                    _connected = value;

                    // 触发连接状态变化事件
                    OnConnectedChanged?.Invoke(this, new ConnectionChangedEventArgs(value));
                }
            }
        }

        public double ChipTemperature { get; private set; }  //芯片温度



        TemperatureController()
        {
        }


        // 定义连接状态变化事件
        public event EventHandler<ConnectionChangedEventArgs> OnConnectedChanged;



        public bool ReConnect(string ip = "127.0.0.1", string port = "3000")
        {
            try
            {
                // 设置 Modbus TCP 从站的 IP 地址和端口号
                string ipAddress = ip;
                int portNum = Convert.ToInt32(port);

                // 创建 Modbus TCP 客户端
                _client = new TcpClient(ipAddress, portNum);
                _master = ModbusIpMaster.CreateIp(_client);
                StartRunningStatusMonitor();
                StartTempratureMonitor();
                Connected = true;
                return true;
            }
            catch (Exception ex)
            {
                LoggerHelper.Info($"ModbusTcp连接失败:{ex.Message}");
                Connected = false;
                return false;
            }
        }

        public void DisConnect()
        {

            // 关闭连接
            _client?.Close();
            Connected = false;
            _isRunning = false;
            OnRunningStatusChangedEventHandler();  // 触发运行状态变化事件
        }

        public void StartRunningStatusMonitor()  // 启动运行状态监控
        {
            Task.Run(() =>
            {
                bool previousRunningStatus = _isRunning;

                while (Connected)
                {
                    try
                    {
                        bool currentRunningStatus = ReadRunningStatus();

                        if (currentRunningStatus != previousRunningStatus)
                        {
                            // Running status has changed
                            _isRunning = currentRunningStatus;
                            previousRunningStatus = currentRunningStatus;

                            // Invoke the event
                            OnRunningStatusChangedEventHandler();
                        }

                        // You might want to add a delay to reduce CPU usage
                        Thread.Sleep(100); // Sleep for 1 second
                    }
                    catch (Exception ex)
                    {
                        LoggerHelper.Info($"读取运行状态失败:{ex.Message}");
                    }
                }
            });
        }

        public void StartTempratureMonitor()
        {
            Task.Run(() =>
            {
                while (Connected)
                {
                    try
                    {
                        double[] temperatures = ReadTemperature();
                        OnTemperatureChangedEventHandler(temperatures);
                        // You might want to add a delay to reduce CPU usage
                        Thread.Sleep(100); // Sleep for 1 second
                    }
                    catch (Exception ex)
                    {
                        LoggerHelper.Info($"空气/产品温度失败:{ex.Message}");
                    }
                }
            });
        }

        /// <summary>
        /// 读取温控设备的运行状态
        /// true表示运行
        /// false表示停止
        /// </summary>
        /// <returns></returns>
        public bool ReadRunningStatus()
        {
            ushort startAddress = 10;
            bool[] res = _master.ReadCoils(startAddress, 1);
            return res[0];
        }

        public double[] ReadTemperature()
        {
            //返回空气温度和表面温度
            ushort startAddress = 0;
            ushort[] res = _master.ReadHoldingRegisters(startAddress, 2);
            double[] result = new double[2];
            int decimalPlaces = 1; //保留一位小数

            result[0] = ConvertUShortToDouble(res[0], decimalPlaces);
            result[1] = ConvertUShortToDouble(res[1], decimalPlaces);

            ChipTemperature = result[1];
            return result;
        }

        private double ConvertUShortToDouble(ushort value, int decimalPlaces)  //处理从温控设备读取的原始温度数据
        {
            if (value > 10000)
            {
                short negativeValue = (short)(value - ushort.MaxValue - 1);  // 负数值
                double result = (double)negativeValue / Math.Pow(10, decimalPlaces);  // 转换为小数
                return result;
            }
            else
            {
                double result = (double)value / Math.Pow(10, decimalPlaces);  // 转换为小数
                return result;
            }


        }


        private void OnRunningStatusChangedEventHandler()  // 运行状态变化事件
        {
            RunningStatusChangedEvent?.Invoke(this, _isRunning);
        }


        private void OnTemperatureChangedEventHandler(double[] values)
        {
            TemperatureChangedEvent?.Invoke(this, values);
        }

        /// <summary>
        /// 打开设备
        /// </summary>
        /// <returns></returns>
        public bool OpenDevice()
        {
            if (!Connected)
            {
                LoggerHelper.Info("未连接设备无法启动");
                return false;
            }
            ushort startAddress = 0;
            bool[] data = new bool[] { true };

            _master.WriteMultipleCoils(startAddress, data);
            //等100毫秒后，判断是否启动成功
            Thread.Sleep(100);
            return ReadRunningStatus();
        }

        /// <summary>
        /// 停止设备
        /// </summary>
        /// <returns></returns>
        public bool CloseDevice()
        {
            if (!Connected)
            {
                LoggerHelper.Info("未连接设备无法关闭");
                return false;
            }
            ushort startAddress = 1;
            bool[] data = new bool[] { true };
            _master.WriteMultipleCoils(startAddress, data);
            //等100毫秒后，判断是否关闭成功
            Thread.Sleep(100);
            return !ReadRunningStatus();
        }
    }


    // 定义连接状态变化事件参数
    public class ConnectionChangedEventArgs : EventArgs
    {
        public bool IsConnected { get; }

        public ConnectionChangedEventArgs(bool isConnected)
        {
            IsConnected = isConnected;
        }
    }
}

using Modbus.Device;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace SLN_Prism.Common.HardWare
{       /// <summary>
        /// 压力传感器类
        /// </summary>
    public class PressureController
    {
        public SerialPort Com { get; set; } = new SerialPort();
        private ModbusSerialMaster master;
        private string rtstr = "0";
        public CancellationTokenSource PressureCts { get; set; } = new CancellationTokenSource();

        public void OpenCom(SerialPortConfig serialPortConfig)
        {
            try
            {
                CloseCom();

                Com.PortName = serialPortConfig.PortName;
                Com.BaudRate = serialPortConfig.BaudRate;
                //com.DataBits = 1;
                //com.Parity = iParity;
                //com.StopBits = iStopBits;

                Com.ReadTimeout = 2000;
                Com.WriteTimeout = 2000;

                Com.Open();
                if (Com.IsOpen)
                {
                    master = ModbusSerialMaster.CreateRtu(Com);
                    if (master == null)
                    {
                        MessageBox.Show($"[createRtuModbus]创建连接失败.");
                    }
                }
                else
                {
                    MessageBox.Show($"[createRtuModbus]打开串口失败.");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(($@"[createRtuModbus]{e.Message}"));
            }
        }

        public void CloseCom()
        {
            if (Com.IsOpen)
            {
                Com.Close();
            }
        }

        public string ReadHoldingRegisters(int nSlaverID, int nStartAddress, int nNumberOfPoints)
        {
            rtstr = "";
            try
            {
                ushort[] rts = master.ReadHoldingRegisters((byte)nSlaverID, (ushort)nStartAddress, (ushort)nNumberOfPoints);
                for (int i = 0; i < rts.Length; i++)
                {
                    if (i > 0) rtstr += ",";
                    rtstr += rts[i].ToString();
                }
            }
            catch (Exception e)
            {
                //MessageBox.Show(($@"[ReadHoldingRegisters]{e.Message}"));
            }

            return rtstr;
        }

        public double ReadPressureValue()
        {
            try
            {
                string value = ReadHoldingRegisters(1, 0, 2);
                string[] pressureStr = value.Split(',');
                int pow = Convert.ToInt32(pressureStr[1]);
                //这里要减一位，读到的第二个数值是小数点位置
                double x = Math.Pow(10, pow - 1);
                double pressureD = Convert.ToDouble(pressureStr[0]) / x;
                return pressureD;
            }
            catch (Exception e)
            {
                PressureCts.Cancel();
                //DialogHelper.Error("压力传感器通讯连接不上，请检查Com口，波特率配置:"+e.Message);
                return 0.000d;
            }
        }

        public void StartRead(CancellationTokenSource cts) //启动异步读取压力传感器数值的方法
        {
            PressureCts = new CancellationTokenSource();
            Task.Run(() =>
            {
                while (!PressureCts.IsCancellationRequested)  //循环读取压力传感器数值
                {
                  //  EventNotificationCenter.Publish(new PressureDataChanged(ReadPressureValue()));  //发布压力传感器数值改变事件
                }
            });
        }
    }
}

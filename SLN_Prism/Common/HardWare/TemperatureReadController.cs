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
{
    /// <summary>
    /// 读取温度传感器类，实例化-OpenCom方法-StartRead方法
    /// </summary>
    public class TemperatureReadController
    {
        public SerialPort Com { get; set; } = new SerialPort();  //创建串口对象
        private ModbusSerialMaster master; //创建Modbus对象
        private string rtstr = "0"; //接收到的字符串
        public CancellationTokenSource ReadTemperatureCts { get; set; } = new CancellationTokenSource();

        public event EventHandler<double[]> TempratureValueChangedEventHandler;

        public double FixtureTemperature { get; private set; }  //夹具温度
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

                Com.ReadTimeout = 2000; //设置读取超时时间
                Com.WriteTimeout = 2000;  //设置写入超时时间

                Com.Open();
                if (Com.IsOpen)
                {
                    master = ModbusSerialMaster.CreateRtu(Com); //创建Modbus主站对象
                    if (master == null)
                    {
                        MessageBox.Show($"温度传感器创建连接失败.");
                    }
                }
                else
                {
                    MessageBox.Show($"温度传感器打开串口失败.");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(($@"温度传感器{e.Message}"));
            }
        }

        public void CloseCom()
        {
            if (Com.IsOpen)
            {
                Com.Close();
            }
        }


        public double[] ReadTempratureValue()
        {
            try
            {
                byte slaveId = 1;
                ushort startAddress = 0;

                ushort[] res = master.ReadInputRegisters(slaveId, startAddress, 4);  //读取4个寄存器的值
                double value1 = ConvertToTemprature(res[0]);
                double value2 = ConvertToTemprature(res[1]);
                double value3 = ConvertToTemprature(res[2]);
                double value4 = ConvertToTemprature(res[3]);
                double[] values = new double[4] { value1, value2, value3, value4 };  //将读取到的4个寄存器的值存入数组
                FixtureTemperature = value2;
                return values;
            }
            catch (Exception e)
            {
                ReadTemperatureCts.Cancel();
                return new double[4] { 0, 0, 0, 0 };
            }
        }

        private double ConvertToTemprature(ushort input)   //模拟量转换温度值
        {
            double result = (input / 249.0) - 4.0;
            result /= 0.076;
            result -= 60;
            result *= 0.98;
            result += 4.5;
            result = Math.Round(result, 1);  //保留一位小数
            return result;

        }

        public void StartRead(CancellationTokenSource cts)  //异步实时读取温度传感器
        {
            ReadTemperatureCts = new CancellationTokenSource();
            Task.Run(() =>
            {
                while (!ReadTemperatureCts.IsCancellationRequested)
                {
                    TempratureValueChangedEventHandler?.Invoke(this, ReadTempratureValue());
                }
            });
        }
    }
}


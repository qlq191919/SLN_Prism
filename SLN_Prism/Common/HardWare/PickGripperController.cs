using Modbus.Device;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SLN_Prism.Common.HardWare
{           /// <summary>
            /// 取料夹爪控制器，串口连接
            /// </summary>
    public class PickGripperController
    {
        public SerialPort Com { get; set; } = new SerialPort();
        private ModbusSerialMaster master;
        private string rtstr = "0";

        public void OpenCom(SerialPortConfig serialPortConfig)
        {
            try
            {
                CloseCom();

                Com.PortName = serialPortConfig.PortName;
                Com.BaudRate = serialPortConfig.BaudRate;
                //com.DataBits
                //com.Parity = iParity;
                //com.StopBits = iStopBits;

                Com.ReadTimeout = 2000;   //设置读取超时时间
                Com.WriteTimeout = 2000;  //设置写入超时时间

                //MyCom.ReceivedBytesThreshold = 1;
                //MyCom.DataReceived += MyCom_DataReceived;

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

        public string ReadHoldingRegisters(int nSlaverID, int nStartAddress, int nNumberOfPoints) //读取保持寄存器数据
        {
            rtstr = "";
            try
            {
                ushort[] rts = master.ReadHoldingRegisters((byte)nSlaverID, (ushort)nStartAddress, (ushort)nNumberOfPoints);
                for (int i = 0; i < rts.Length; i++) //将读取到的保持寄存器数据转换为字符串，并用逗号分隔
                {
                    if (i > 0) rtstr += ",";
                    rtstr += rts[i].ToString();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(($@"[ReadHoldingRegisters]{e.Message}"));
            }

            return rtstr;
        }

        public void GripperPick(int strength, int position, int speed)   //控制夹爪抓取
        {
            try
            {
                //写入力
                master?.WriteSingleRegister(1, Convert.ToUInt16("0x0101", 16), Convert.ToUInt16(strength));
                //写入速度
                master?.WriteSingleRegister(1, Convert.ToUInt16("0x0104", 16), Convert.ToUInt16(speed));
                //写入位置
                master?.WriteSingleRegister(1, Convert.ToUInt16("0x0103", 16), Convert.ToUInt16(position));
            }
            catch (Exception e)
            {
                MessageBox.Show(($@"[WriteSingleRegister]{e.Message}"));
            }
        }

        public bool CheckJackerOk()
        {
            string result = ReadHoldingRegisters(1, Convert.ToUInt16("0x0201", 16), 1);
            if ("3"!.Equals(result))
            {
                //（无此类引用发布消息）  EventNotificationCenter.Publish(new MessageEvent("检测到夹爪2物体掉落"));
                return false;
            }
            return true;
        }

        public void InitGripper() //通过 Modbus 协议向特定的寄存器地址写入数据，用于初始化夹爪控制器
        {
            try
            {
                //参数1为设备地址，参数2为寄存器地址，参数3为写入的数据
                master?.WriteSingleRegister(1, Convert.ToUInt16("0x0100", 16), (ushort)1);
            }
            catch (Exception e)
            {
                MessageBox.Show(($@"[WriteSingleRegister]{e.Message}"));
            }
        }
    }
}

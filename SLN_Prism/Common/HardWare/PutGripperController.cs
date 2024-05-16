using Modbus.Device;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SLN_Prism.Common.HardWare
{/// <summary>
/// 放料夹爪控制器
/// </summary>
    public class PutGripperController
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

                Com.ReadTimeout = 2000;
                Com.WriteTimeout = 2000;

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
                MessageBox.Show(($@"[ReadHoldingRegisters]{e.Message}"));
            }

            return rtstr;
        }

        public bool CheckJackerOk()
        {
            string result = ReadHoldingRegisters(1, Convert.ToUInt16("0x0201", 16), 1);
            if ("3"!.Equals(result))
            {
                //（无此类引用）EventNotificationCenter.Publish(new MessageEvent("检测到夹爪1物体掉落"));
                return false;
            }
            return true;
        }

        public void GripperPick(int strength, int position, int speed)
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

        public void InitGripper() //通过 Modbus 协议向特定的寄存器地址写入数据，用于初始化夹爪控制器
        {
            {
                try
                {
                    master?.WriteSingleRegister(1, Convert.ToUInt16("0x0100", 16), (ushort)1);
                }
                catch (Exception e)
                {
                    MessageBox.Show(($@"[WriteSingleRegister]{e.Message}"));
                }
            }
        }
    }
}

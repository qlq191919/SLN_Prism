using Modbus.Device;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SLN_Prism.Common.HardWare
{
    public class ModbusRtu
    {
        private SerialPort com = new SerialPort();
        private ModbusSerialMaster master;
        string rtstr = "0";

        public void OpenCom(SerialPortConfig serialPortConfig)
        {
            try
            {
                CloseCom();

                com.PortName = serialPortConfig.PortName;
                com.BaudRate = serialPortConfig.BaudRate;
                //com.DataBits 
                //com.Parity = iParity;
                //com.StopBits = iStopBits;

                com.ReadTimeout = 2000;
                com.WriteTimeout = 2000;

                //MyCom.ReceivedBytesThreshold = 1;
                //MyCom.DataReceived += MyCom_DataReceived;

                com.Open();
                if (com.IsOpen)
                {
                    master = ModbusSerialMaster.CreateRtu(com);
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
            if (com.IsOpen)
            {
                com.Close();
            }
        }

        public string ReadHoldingRegisters(int nSlaverID, int nStartAddress, int nNumberOfPoints)
        {
            rtstr = "";
            try
            {
                // 看这个函数解释就是 holding register 保持型寄存器
                // master.WriteSingleRegister 
                //master.WriteSingleRegister(1, Convert.ToUInt16("0x0405", 10), 300);
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

        public void WriteSingleRegister(int nSlaverID, int nStartAddress, int nNumberOfPoints)
        {
            throw new NotImplementedException();
        }

    }
}

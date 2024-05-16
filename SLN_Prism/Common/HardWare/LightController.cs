using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SLN_Prism.Common.HardWare
{
    public class LightController
    {
        public SerialPort Com { get; set; } = new SerialPort();
        /// <summary>
        /// 打开COM口
        /// </summary>
        public void OpenCom(SerialPortConfig serialPortConfig)
        {
            try
            {
                CloseCom();
                Com.PortName = serialPortConfig.PortName;
                Com.BaudRate = serialPortConfig.BaudRate;
                Com.ReadTimeout = 2000;
                Com.WriteTimeout = 2000;
                Com.Open();
                if (Com.IsOpen)
                {
                }
                else
                {
                    MessageBox.Show($"[openCom]打开串口失败.");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(($@"[openCom]{e.Message}"));
            }
        }


        public void CloseCom()
        {
            if (Com.IsOpen)
            {
                Com.Close();
            }
        }

        private static string[] LightEnabled = new string[4]
        {
            "0000","0000","0000","0000"
        };

        /// <summary>
        /// 打开上相机光源
        /// </summary>
        /// <param name="brights"></param>
        /// <returns></returns>
        public ActionResult SetUpLight(int bright)
        {
            if (Com == null)
            {
                return new ActionResult(-2, "光源设置失败，串口未打开");
            }
            LightEnabled = new string[4]
            {
                "0000","0000","0000","0000"
            };
            LightEnabled[0] = bright.ToString().PadLeft(4, '0');
            LightEnabled[1] = bright.ToString().PadLeft(4, '0');//上
            LightEnabled[2] = bright.ToString().PadLeft(4, '0');//下
            LightEnabled[3] = bright.ToString().PadLeft(4, '0');
            ////0 上 1下 2右 3左
            for (int i = 0; i < LightEnabled.Length; i++)
            {
                Channel channel = new Channel(i);
                channel.Brightness = LightEnabled[i];
                string cmd = channel.GetChannelData();
                var data = Encoding.ASCII.GetBytes(cmd);
                Com?.Write(data, 0, cmd.Length);
            }

            return ActionResult.OK;
        }
        /// <summary>
        /// 打开下相机光源
        /// </summary>
        /// <param name="brights"></param>
        /// <returns></returns>
        public ActionResult SetDownLight(int bright)
        {
            if (Com == null)
            {
                return new ActionResult(-2, "光源设置失败，串口未打开");
            }
            LightEnabled = new string[4]
            {
                "0000","0000","0000","0000"
            };
            LightEnabled[0] = bright.ToString().PadLeft(4, '0');
            LightEnabled[1] = bright.ToString().PadLeft(4, '0');
            LightEnabled[2] = bright.ToString().PadLeft(4, '0');
            LightEnabled[3] = bright.ToString().PadLeft(4, '0');
            ////0 上 1下 2右 3左
            for (int i = 0; i < LightEnabled.Length; i++)
            {
                Channel channel = new Channel(i);
                channel.Brightness = LightEnabled[i];
                string cmd = channel.GetChannelData();
                var data = Encoding.ASCII.GetBytes(cmd);
                Com.Write(data, 0, cmd.Length);
            }

            return ActionResult.OK;
        }

        public ActionResult CloseLight()
        {
            if (Com == null)
            {
                return new ActionResult(-2, "开启灯光失败，串口未打开");
            }
            LightEnabled = new string[4]
            {
                "0000","0000","0000","0000"
            };

            ////0 上 1下 2右 3左
            for (int i = 0; i < LightEnabled.Length; i++)
            {
                Channel channel = new Channel(i);
                channel.Brightness = LightEnabled[i];
                string cmd = channel.GetChannelData();
                var data = Encoding.ASCII.GetBytes(cmd);
                Com.Write(data, 0, cmd.Length);
            }

            return ActionResult.OK;
        }
    }
}

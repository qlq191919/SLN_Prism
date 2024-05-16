using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLN_Prism.Common.HardWare
{
    public class SerialPortConfig
    {
        public string PortName { get; set; } = "COM1";
        public int BaudRate { get; set; } = 9600;
        public string EncodingName { get; set; } = "US-ASCII";
        //public string NewLine { get; set; } = "\r";

        public void Apply(SerialPort port)  // Apply the configuration to a SerialPort object
        {
            if (port == null)
                return;
            var config = this;
            port.PortName = config.PortName;
            port.BaudRate = config.BaudRate;
            //port.NewLine = config.NewLine;
            port.Encoding = Encoding.GetEncoding(EncodingName);
        }
    }
}

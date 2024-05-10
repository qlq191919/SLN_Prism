using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLN_Prism.ChipModels
{
    public class ChipInfo
    {
        public string Name { get; set; }

        public string Status { get; set; }
        public string Time { get; set; }

        public string Id { get; set; }
        public string Result { get; set; }

        //压力值
        public string Pressure { get; set; }

        //芯片所在行列数
        public string RowColumn { get; set; }

        public float X { get; set; }

        public float Y { get; set; }

        public float Angle { get; set; }
    }
}

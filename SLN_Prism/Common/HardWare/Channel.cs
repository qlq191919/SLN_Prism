using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLN_Prism.Common.HardWare
{
    public class Channel
    {
        private readonly string _channel;

        private float _level;
        private bool _on;
        private string _brightness;

        public Channel(int channelIndex)
        {
            switch (channelIndex)
            {
                case 0: _channel = "A"; break;
                case 1: _channel = "B"; break;
                case 2: _channel = "C"; break;
                case 3: _channel = "D"; break;
            }
        }

        public bool On
        {
            get => _on;
            set => _on = value;
        }

        public float Level
        {
            get => _level;
            set
            {
                if (Math.Abs(_level - value) < 0.0005f)
                    return;
                if (value < 0) value = 0;
                if (value > 1) value = 1;
                _level = value;
            }
        }

        public String Brightness { get => _brightness; set => _brightness = value; }

        protected int GetLevel()
        {
            return (int)Math.Round(Level * 999);
        }

        public string GetChannelData()
        {
            return $"S{_channel}{Brightness}#";
        }
    }
}

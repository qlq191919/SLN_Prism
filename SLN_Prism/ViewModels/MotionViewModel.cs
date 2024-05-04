using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLN_Prism.ViewModels
{
    class MotionViewModel : BindableBase
    {
           
             private string _customPositionName;
        public string CustomPositionName
        {
            get { return _customPositionName; }
            set { _customPositionName = value; RaisePropertyChanged(); }
        }
    }
}


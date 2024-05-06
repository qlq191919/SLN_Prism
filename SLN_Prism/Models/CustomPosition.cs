using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SLN_Prism.Models
{
    public class CustomPosition:BindableBase
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; RaisePropertyChanged(); }
        }

        private double _position;
        public double Position
        {
            get { return _position; }
            set {_position = value; RaisePropertyChanged(); }
        }
    }
}

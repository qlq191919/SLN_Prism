using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

    public class CustomPositionCollection : BindableBase
    {
        private ObservableCollection<CustomPosition> _positions;
        public ObservableCollection<CustomPosition> Positions
        {
            get { return _positions; }
            set { _positions = value; RaisePropertyChanged(); }
        }
    }

    public class CustomPositionCombine:BindableBase
    {
        private ObservableCollection<CustomPositionCollection> _positionsCollections;
        public ObservableCollection<CustomPositionCollection> PositionsCollections
        {
            get { return _positionsCollections; }
            set { _positionsCollections = value; RaisePropertyChanged(); }
        }
    }
}

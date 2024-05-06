using Prism.Commands;
using Prism.Mvvm;
using SLN_Prism.Models;
using SLN_Prism.Tool;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SLN_Prism.ViewModels
{
    class MotionViewModel : BindableBase
    {

        public MotionViewModel()
        {
            CustomPositions = SC.ReadCustomPositionToList(1);
            CustomPositionsGrid = SC.ReadCustomPosition(1);
            SaveCustomPositionCommand = new DelegateCommand(SaveCustomPosition);
            //SelectCustomPositionCommand = new DelegateCommand(() => DPosition = Convert.ToDouble(SelectedPosition));
        }
        SaveCustomPosition SC = new SaveCustomPosition(@"D:\TEST");
       

        private ObservableCollection<CustomPosition> _customPositions;
        public ObservableCollection<CustomPosition> CustomPositions
        {
            get { return _customPositions; }
            set { _customPositions = value; RaisePropertyChanged(); }
        }
        private string _customPositionNameInput;
        public string CustomPositionNameInput
        {
            get { return _customPositionNameInput; }
            set { _customPositionNameInput = value; RaisePropertyChanged(); }
        }

        private double _dPosition;
        public double DPosition
        {
            get { return _dPosition; }
            set { _dPosition = value; RaisePropertyChanged(); }
        }
       
        private double _selectPosition;
        public double SelectedPosition
        {
            get { return _selectPosition; }
            set { _selectPosition = value; RaisePropertyChanged(); }
        }

        private double _customPositionInput;
        public double CustomPositionInput
        {
            get { return _customPositionInput; }
            set
            {
                if (value >= -1000 && value <= 1000) _customPositionInput = value;
                else MessageBox.Show("请输入合理的坐标值（-1000~1000）"); RaisePropertyChanged();
            }
        }

        public DelegateCommand SaveCustomPositionCommand { get; set; }
        public void SaveCustomPosition()
        {
            CustomPositions.Add(new CustomPosition { Name = CustomPositionNameInput, Position = CustomPositionInput });
            SC.WriteCustomPosition(CustomPositionNameInput, CustomPositionInput, 1);
            CustomPositionsGrid = SC.ReadCustomPosition(1);
            CustomPositionNameInput="";
            CustomPositionInput=0;
        }
        
        public DelegateCommand SelectCustomPositionCommand { get; set; }
          
        

        private DataTable _customPositionsGrid;
        public DataTable CustomPositionsGrid
        {
            get { return _customPositionsGrid; }
            set { _customPositionsGrid = value; RaisePropertyChanged(); }
        }

        //private bool _isCustomPositionSelected;
        //public bool IsCustomPositionSelected
        //{
        //    get { return _isCustomPositionSelected; }
        //    set { _isCustomPositionSelected = value; RaisePropertyChanged(); }
        //}



    }
}


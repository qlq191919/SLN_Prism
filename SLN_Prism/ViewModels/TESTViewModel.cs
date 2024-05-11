using Prism.Commands;
using Prism.Mvvm;
using SLN_Prism.Tool;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SLN_Prism.Common;

namespace SLN_Prism.ViewModels
{
    class TESTViewModel : BindableBase
    {
        public TESTViewModel()
        {
           
            
            
            logWriteCommand = new DelegateCommand(() =>
            {
               LoggerHelper.Info(Text1);
                LoggerHelper.Error(Text2);
                LoggerHelper.Debug("Debug");
            });
            logReadCommand = new DelegateCommand(() =>
            {
              
            });
            historyAlarmCommand = new DelegateCommand(() =>
            {
                
            }); 

        }
        private DataTable _dtdate;
        public DataTable Dtdate
        {
            get { return _dtdate; }
            set { _dtdate = value; RaisePropertyChanged(); }
        }
        private string _text1;
        public string Text1
        {
            get { return _text1; }
            set { _text1 = value; RaisePropertyChanged(); }
        }
        private string _text2;
        public string Text2
        {
            get { return _text2; }
            set { _text2 = value; RaisePropertyChanged(); }
        }

        public DelegateCommand logWriteCommand { get; private set; }
        public DelegateCommand logReadCommand { get; private set; }
        public DelegateCommand historyAlarmCommand { get; private set; }

    }
}

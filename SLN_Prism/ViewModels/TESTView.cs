using Prism.Commands;
using Prism.Mvvm;
using SLN_Prism.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLN_Prism.ViewModels
{
    class TESTView : BindableBase
    {
        public TESTView()
        {
            logging  log= new logging(@"D:\TEST");
            historyAlarm history = new historyAlarm(@"D:\TEST");
            logWriteCommand = new DelegateCommand(() =>
            {
                //TODO: Do something here
            });
            logReadCommand = new DelegateCommand(() =>
            {
                //TODO: Do something here
            });
            historyAlarmCommand = new DelegateCommand(() =>
            {
                //TODO: Do something here
            }); 

        }
        private string _testPro;
        public string TestPro
        {
            get { return TestPro; }
            set { TestPro = value; RaisePropertyChanged(); }
        }

        public DelegateCommand logWriteCommand { get; private set; }
        public DelegateCommand logReadCommand { get; private set; }
        public DelegateCommand historyAlarmCommand { get; private set; }

    }
}

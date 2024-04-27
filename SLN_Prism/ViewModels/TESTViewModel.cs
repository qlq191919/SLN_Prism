using Prism.Commands;
using Prism.Mvvm;
using SLN_Prism.Tool;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLN_Prism.ViewModels
{
    class TESTViewModel : BindableBase
    {
        public TESTViewModel()
        {
            logging  log= new logging(@"D:\TEST\log.txt");
            historyAlarm history = new historyAlarm(@"D:\TEST\history.txt");
            
            logWriteCommand = new DelegateCommand(() =>
            {
               log.WriteLog(Text1,Text2);
            });
            logReadCommand = new DelegateCommand(() =>
            {
               Dtdate = log.ReadLogDt(DateTime.Now.AddDays(-1),DateTime.Now);
            });
            historyAlarmCommand = new DelegateCommand(() =>
            {
                history.ReadLogDt(DateTime.Now.AddDays(-1), DateTime.Now);
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

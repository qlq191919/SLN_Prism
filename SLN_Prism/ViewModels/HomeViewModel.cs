using NLog;
using Prism.Commands;
using Prism.Mvvm;
using SLN_Prism.Common;
using SLN_Prism.Common.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SLN_Prism.ViewModels
{
    public class HomeViewModel:BindableBase
    {
       
        public HomeViewModel()
        {
           LogText = new ObservableCollection<Alarms>();
            TestCommand = new DelegateCommand(() =>
            {
               
                
                    LoggerHelper.Error("T Error");                  
                LoggerHelper.Error("T1111 Error");
                LoggerHelper.Error("T Error");
                LoggerHelper.Error("T1111 Error"); 
                LoggerHelper.Error("T Error");
                LoggerHelper.Error("T1111 Error");

            });
        }
        public static ObservableCollection<Alarms> LogText { get;set; }
        public DelegateCommand TestCommand { get; private set; }



    }
}

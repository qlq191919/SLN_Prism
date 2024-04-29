using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLN_Prism.ViewModels { 
    class MDViewModel : BindableBase
{
        public MDViewModel()
        {
            Title = "TEST";
            Message = "This is a readonly message.";
            UpdateCommand = new DelegateCommand(() => { Message = "updated message.\r\n"; });
            //复合命令
            UpdateCommand1 = new DelegateCommand(() => { Message += "UPDATED message.\r\n"; });
            UpdateCommand2 = new DelegateCommand(() => { Message += "UPDATED2 message."; });
            AllCommand = new CompositeCommand();
            AllCommand.RegisterCommand(UpdateCommand1);
            AllCommand.RegisterCommand(UpdateCommand2);
        }
        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private string _message;
        public string Message
        {
            get { return _message; }
            set { _message = value; RaisePropertyChanged(); }
        }

        public DelegateCommand UpdateCommand { get; private set; }
        public DelegateCommand UpdateCommand1 { get; private set; }
        public DelegateCommand UpdateCommand2 { get; private set; }
        public CompositeCommand AllCommand { get; private set; }

    }
}

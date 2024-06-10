using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace SLN_Prism.ViewModels
{
    public class LoginViewModel : BindableBase, IDialogAware
    {
        public string Title { get; set; } = "全功能探台测试设备"; // 标题属性

        public event Action<IDialogResult> RequestClose;  // 请求关闭对话框的事件

        public LoginViewModel()
        {
            ExecuteCommand = new DelegateCommand<string>(Execute);  // 登录命令
        }

        void Execute(string arg)
        {
            switch (arg)
            {
                case "Login":
                    Login();
                    break;
                case "LoginOut":
                    LoginOut();
                    break;
                default:
                    break;
            }
        }

         

        public bool CanCloseDialog()  // 是否可以关闭对话框的方法
        {
            return true;
        }

        public void OnDialogClosed()  // 对话框关闭时的方法
        {

        }

        public void OnDialogOpened(IDialogParameters parameters)  // 对话框打开时的方法
        {

        }

        public DelegateCommand<string> ExecuteCommand { get; private set; }

        private string _username;
        public string Username
        {
            get { return _username; }
            set { _username = value; RaisePropertyChanged(); }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set { _password = value; RaisePropertyChanged(); }
        }

        private bool _rememberMe;
        public bool RememberMe
        {
            get { return _rememberMe; }
            set { _rememberMe = value; RaisePropertyChanged(); }
        }


        private string _errorMessage;
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; RaisePropertyChanged(); }
        }

        private bool _isLoggedIn;
        public bool IsLoggedIn
        {
            get { return _isLoggedIn; }
            set { _isLoggedIn = value; RaisePropertyChanged(); }
        }

        public void Login()  // 登录方法
        {
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                ErrorMessage = "用户名或密码不能为空";
                return;
            }

            if (Username == "admin" && Password == "123456")
            {
                IsLoggedIn = true;
                RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
            }
            else
            {
                ErrorMessage = "用户名或密码错误";
            }

        }
        void LoginOut()
        {
            IsLoggedIn = false;
            RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
        }
    }
}

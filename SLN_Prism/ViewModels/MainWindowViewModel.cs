using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using SLN_Prism.Common;
using SLN_Prism.Common.Models;
using SLN_Prism.Extensions;
using SLN_Prism.Views;
using System.Collections.ObjectModel;

namespace SLN_Prism.ViewModels
{
    public class MainWindowViewModel : BindableBase,IConfigureService
    {
        private string userName;

        public string UserName
        {
            get { return userName; }
            set { userName = value; RaisePropertyChanged(); }
        }
        public DelegateCommand LoginOutCommand { get; private set; }
        public DelegateCommand<MenuBar> NavigateCommand { get; private set; }
        public DelegateCommand GoBackCommand { get; private set; }
        public DelegateCommand GoForwardCommand { get; private set; }

       
        private readonly IContainerProvider containerProvider;
        private readonly IRegionManager regionManager;
        private IRegionNavigationJournal journal;

        public MainWindowViewModel(IRegionManager regionManager,IContainerProvider containerProvider)
        {
           MenuBars = new ObservableCollection<MenuBar>();
           
            NavigateCommand = new DelegateCommand<MenuBar>(Navigate);
            GoBackCommand = new DelegateCommand(() =>
            {
                if (journal != null && journal.CanGoBack)
                    journal.GoBack();
            });
            GoForwardCommand = new DelegateCommand(() =>
            {
                if (journal != null && journal.CanGoForward)
                    journal.GoForward();
            });
            //LoginOutCommand = new DelegateCommand(() =>
            //{
            //    //注销当前用户
            //    App.LoginOut(containerProvider);
            //});
            //this.containerProvider = containerProvider;
            this.regionManager = regionManager;

        }
        /// <summary>
        /// 命令，导航到指定页面，传入<MenuBar>对象的NameSpcae作为参数
        /// </summary>
        /// <param name="obj"></param>
        private void Navigate(MenuBar obj)
        {
            if (obj == null || string.IsNullOrWhiteSpace(obj.NameSpace))
                return;

            regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate(obj.NameSpace, back =>
            {
                journal = back.Context.NavigationService.Journal;
            });
        }
        /// <summary>
        /// 建立菜单集合
        /// </summary>
        private ObservableCollection<MenuBar> menuBars;
        public ObservableCollection<MenuBar> MenuBars
        {
            get { return menuBars; }
            set { menuBars = value; RaisePropertyChanged(); }
        }
        /// <summary>
        /// 创建菜单栏
        /// </summary>
        void CreateMenuBar()
        {
            MenuBars.Add(new MenuBar() { Title = "首页", Icon = "Home", NameSpace="Home"});
            MenuBars.Add(new MenuBar() { Title = "操作界面", Icon = "Controller", NameSpace = "Operation" });
            MenuBars.Add(new MenuBar() { Title = "视觉界面", Icon = "Camera", NameSpace= "Vision" });
            MenuBars.Add(new MenuBar() { Title = "单轴界面", Icon = "AccountCogOutline", NameSpace="Motion"});
            MenuBars.Add(new MenuBar() { Title = "IO监视", Icon = "MonitorEye", NameSpace="Monitoring"});
            MenuBars.Add(new MenuBar() { Title = "报警记录", Icon = "AlarmLight", NameSpace = "Alarm" });
            MenuBars.Add(new MenuBar() { Title = "设置", Icon = "CogOutline", NameSpace="Settings"});
            MenuBars.Add(new MenuBar() { Title = "测试", Icon = "AlphaTCircle", NameSpace = "TEST" });
        }
        /// <summary>
        /// 配置首页初始化参数
        /// </summary>
        public void Configure()
        {
            UserName = AppSession.UserName;
            CreateMenuBar();
            regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate("Home");
        }
    }
}

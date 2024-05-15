using ModuleA;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Services.Dialogs;
using SLN_Prism.Common;
using SLN_Prism.ViewModels;
using SLN_Prism.Views;
using System;
using System.Windows;

namespace SLN_Prism
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App: PrismApplication
    {
        /// <summary>
        /// 从容器中解析实例，作为主窗口
        /// </summary>
        /// <returns></returns>
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }
        /// <summary>
        /// 初始化
        /// </summary>
        protected override void OnInitialized()
        {
           

                var service = App.Current.MainWindow.DataContext as IConfigureService;
                if (service != null)
                    service.Configure();
                base.OnInitialized();
           
            }

        
        /// <summary>
        /// 注册视图和视图间的依赖关系
        /// </summary>
        /// <param name="containerRegistry"></param>
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<Home, HomeViewModel>();
            containerRegistry.RegisterForNavigation<Motion, MotionViewModel>();
            containerRegistry.RegisterForNavigation<Operation,OperationViewModel>();
            containerRegistry.RegisterForNavigation<Settings, SettingsViewModel>();
            containerRegistry.RegisterForNavigation<Monitoring, MonitoringViewModel>();
            containerRegistry.RegisterForNavigation<Vision, VisionViewModel>();
            containerRegistry.RegisterForNavigation<Alarm, AlarmViewModel>();
            containerRegistry.RegisterForNavigation<TEST, TESTViewModel>();
            containerRegistry.RegisterForNavigation<Login, LoginViewModel>();

            
        }
        /// <summary>
        /// 注册模块
        /// </summary>
        /// <param name="moduleCatalog"></param>
        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<ModuleAModule>();
            base.ConfigureModuleCatalog(moduleCatalog);
        }
    }
}

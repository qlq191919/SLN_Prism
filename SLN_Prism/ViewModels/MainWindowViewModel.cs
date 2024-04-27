using Prism.Mvvm;
using Prism.Regions;
using SLN_Prism.Views;

namespace SLN_Prism.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "SLN";
        private readonly IRegionManager _regionManager;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public MainWindowViewModel(IRegionManager regionManager)
        {
            this._regionManager = regionManager;
            //_regionManager.RegisterViewWithRegion("MainRegion", typeof(MD));
           _regionManager.RegisterViewWithRegion("MainRegion", typeof(TEST));
            _regionManager.RegisterViewWithRegion("LeftRegion", typeof(Navigation));
            // _regionManager.RequestNavigate("ContentRegion", "NavigationPage");
        }
    }
}

using Prism.Mvvm;
using Infrastructure.Constants;
using ModuleEdit.Views;
using Prism.Logging;
using Prism.Regions;
using RSSReader.Views.AddFeed;

namespace RSSReader.ViewModels
{
    public class SecondWindowViewModel : BindableBase
    {
        private readonly ILoggerFacade _logger = ProjectLogger.GetLogger;
        private readonly IRegionManager _regionManager;

        public SecondWindowViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            _regionManager.RequestNavigate(RegionNames.ContentNewWindow, nameof(AddFeedFormUserControl));
            // oder das hier _regionManager.RequestNavigate(RegionNames.ContentNewWindow, nameof(EditFeedFormUserControl));
        }
    }
}

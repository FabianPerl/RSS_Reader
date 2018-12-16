using Infrastructure.Constants;
using ModuleFeeds.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace ModuleFeeds
{
    public class ModuleFeedsModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public ModuleFeedsModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            // _regionManager.RequestNavigate(RegionNames.ContentRegionLeft, nameof(FeedBoxUserControl)); 
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<FeedBoxUserControl>();
        }
    }
}
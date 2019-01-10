using ModuleFeeds.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace ModuleFeeds
{
    public class ModuleFeedsModule : IModule
    {
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
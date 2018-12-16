using Infrastructure.Constants;
using ModuleArchiveFeeds.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace ModuleArchiveFeeds
{
    public class ModuleArchiveFeedsModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public ModuleArchiveFeedsModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RequestNavigate(RegionNames.ContentRegionLeft, nameof(ArchiveFeedBoxUserControl));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ArchiveFeedBoxUserControl>();
        }
    }
}
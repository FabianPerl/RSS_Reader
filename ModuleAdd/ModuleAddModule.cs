using Infrastructure.Constants;
using ModuleAdd.ViewModels;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using RSSReader.Views.AddFeed;

namespace ModuleAdd
{
    public class ModuleAddModule : IModule
    {
        private readonly IRegionManager _regionManager;
        public ModuleAddModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RequestNavigate(RegionNames.ContentNewWindow, nameof(AddFeedFormUserControl));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<AddFeedFormUserControl>();
        }
    }
}
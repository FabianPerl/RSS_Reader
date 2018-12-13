using Infrastructure.Constants;
using ModuleBrowser.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace ModuleBrowser
{
    public class ModuleBrowserModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public ModuleBrowserModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RequestNavigate(RegionNames.ContentRegionRight, nameof(ViewA));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ViewA>(); 
        }
    }
}
using ModuleAdd.ViewModels;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

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
            _regionManager.RequestNavigate("ContentNewWindow", nameof(AddFeedFormUserControlViewModel));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<AddFeedFormUserControlViewModel>();
        }
    }
}
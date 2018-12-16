using Infrastructure.Constants;
using ModuleEdit.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace ModuleEdit
{
    public class ModuleEditModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public ModuleEditModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }
        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<EditFeedFormUserControl>();
        }
    }
}
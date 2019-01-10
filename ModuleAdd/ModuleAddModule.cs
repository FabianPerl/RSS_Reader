using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace ModuleAdd
{
    public class ModuleAddModule : IModule
    {
        public ModuleAddModule()
        {
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
          //  _regionManager.RequestNavigate(RegionNames.ContentNewWindow, nameof(AddFeedFormUserControl));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
         //   containerRegistry.RegisterForNavigation<AddFeedFormUserControl>();
        }
    }
}
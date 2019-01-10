using Prism.Ioc;
using Prism.Modularity;

namespace ModuleAdd
{
    public class ModuleAddModule : IModule
    {
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
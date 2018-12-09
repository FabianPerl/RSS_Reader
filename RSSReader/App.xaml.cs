using Prism.Ioc;
using System.Windows;
using ModuleAdd;
using ModuleBrowser;
using ModuleEdit;
using ModuleFeeds;
using Prism.Modularity;
using RSSReader.Views;

namespace RSSReader
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            //for the first window
            moduleCatalog.AddModule(typeof(ModuleFeedsModule), InitializationMode.WhenAvailable);
            moduleCatalog.AddModule(typeof(ModuleBrowserModule), InitializationMode.WhenAvailable);

            //for the secondary window
            moduleCatalog.AddModule(typeof(ModuleAddModule), InitializationMode.WhenAvailable);
            moduleCatalog.AddModule(typeof(ModuleEditModule), InitializationMode.WhenAvailable);
        }
    }
}

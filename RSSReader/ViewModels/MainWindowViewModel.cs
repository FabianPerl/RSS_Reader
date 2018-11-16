using Prism.Mvvm;

namespace RSSReader.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "RSS Reader";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public MainWindowViewModel()
        {

        }
    }
}

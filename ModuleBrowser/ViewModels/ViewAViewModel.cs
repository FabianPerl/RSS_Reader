using System;
using Prism.Mvvm;

namespace ModuleBrowser.ViewModels
{
    public class ViewAViewModel : BindableBase
    {
        private Uri _currentUri;

        public ViewAViewModel()
        {
            CurrentUri = new Uri("https://www.heise.de");
        }

        /// <summary>
        /// set and get the current uri that is shown in the window
        /// </summary>
        public Uri CurrentUri
        {
            get => _currentUri;
            set => SetProperty(ref _currentUri, value);
        }
    }
}

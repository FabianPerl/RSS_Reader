using System;
using Infrastructure.Events;
using Prism.Events;
using Prism.Mvvm;

namespace ModuleBrowser.ViewModels
{
    public class ViewAViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;
        private Uri _currentUri;

        public ViewAViewModel(IEventAggregator eventAggregator)
        {
            CurrentUri = new Uri("https://www.heise.de");
            eventAggregator.GetEvent<WantUriEvent>().Subscribe(SetTheUri);
        }

        private void SetTheUri(Uri uri)
        {
            CurrentUri = uri;
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

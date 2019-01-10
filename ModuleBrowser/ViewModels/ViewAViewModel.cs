using System;
using Infrastructure.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;

namespace ModuleBrowser.ViewModels
{
    public class ViewAViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;
        private Uri _currentUri;

        public DelegateCommand CloseBrowserDelegateCommand { get; set; }

        public ViewAViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            CloseBrowserDelegateCommand = new DelegateCommand(CloseBrowser);
            
            _eventAggregator.GetEvent<WantUriEvent>().Subscribe(SetTheUri);
        }

        private void CloseBrowser()
        {
            _eventAggregator.GetEvent<WantCloseUriEvent>().Publish();
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

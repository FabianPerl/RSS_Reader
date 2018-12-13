using System;
using Infrastructure.Constants;
using Infrastructure.Events;
using ModuleBrowser.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

namespace ModuleBrowser.ViewModels
{
    public class ViewAViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IRegionManager _regionManager;
        private Uri _currentUri;

        public DelegateCommand CloseBrowserDelegateCommand { get; set; }

        public ViewAViewModel(IEventAggregator eventAggregator, IRegionManager regionManager)
        {
            _regionManager = regionManager;
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

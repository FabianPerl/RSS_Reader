using System;
using Infrastructure.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;

namespace ModuleBrowser.ViewModels
{
    /// <summary>
    /// ViewModel for the browser
    /// </summary>
    public class ViewAViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;
        private Uri _currentUri;

        /// <summary>
        /// Handles the interaction that the user wants to close the browser
        /// </summary>
        public DelegateCommand CloseBrowserDelegateCommand { get; }

        public ViewAViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            CloseBrowserDelegateCommand = new DelegateCommand(CloseBrowser);
            
            _eventAggregator.GetEvent<WantUriEvent>().Subscribe(SetTheUri);
        }

        /// <summary>
        /// Publishes an event that the uri should be closed
        /// </summary>
        private void CloseBrowser()
        {
            _eventAggregator.GetEvent<WantCloseUriEvent>().Publish();
        }

        /// <summary>
        /// Sets the uri for the browser
        /// </summary>
        /// <param name="uri">The Uri that should be shown</param>
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

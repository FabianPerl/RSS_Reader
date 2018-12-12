using System;
using Infrastructure.Constants;
using Infrastructure.Events;
using ModuleBrowser.Views;
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

        public ViewAViewModel(IEventAggregator eventAggregator, IRegionManager regionManager)
        {
            _regionManager = regionManager;
            
            eventAggregator.GetEvent<WantUriEvent>().Subscribe(SetTheUri);
        }

        private void SetTheUri(Uri uri)
        {
            try
            {
               //CurrentUri = new Uri("https://www.heise.de");
                CurrentUri = uri;

                //CurrentUri = uri;
            } catch(Exception e)
            {
                
            }
        
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

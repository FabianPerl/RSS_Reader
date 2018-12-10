using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Infrastructure.Constants;
using Infrastructure.Events;
using Infrastructure.Models;
using Infrastructure.Services;
using Infrastructure.ViewModels;
using Prism.Commands;
using Prism.Events;
using Prism.Logging;
using Prism.Mvvm;
using Prism.Regions;

namespace ModuleFeeds.ViewModels
{
    public class FeedBoxUserControlViewModel : BindableBase
    {
        private readonly ILoggerFacade _logger = ProjectLogger.GetLogger;
        private readonly IEventAggregator _eventAggregator;
        private Uri _lastFeedUri;
        private readonly IFeedService _feedService = new FeedServiceImpl();
        public DelegateCommand<FeedViewModel> ChangeFeedCommand { get; }

       
        public FeedBoxUserControlViewModel(IEventAggregator eventAggregator)
        {
            UpdateFeedList(new Uri("https://www.heise.de/newsticker/heise-atom.xml"));
            ChangeFeedCommand = new DelegateCommand<FeedViewModel>(ClickedFeedView);
            _eventAggregator = eventAggregator;

            eventAggregator.GetEvent<FetchDataEvent>().Subscribe(ShouldUpdateFeedList);
            eventAggregator.GetEvent<WantFeedEvent>().Subscribe(UpdateFeedList);
            eventAggregator.GetEvent<WantAllFeedsEvent>().Subscribe(UpdateFeedList);
        }

        #region attributes
        /// <summary>
        /// all feeds that will be shown from a source 
        /// </summary>
        public ObservableCollection<FeedViewModel> AllFeeds { get; set; } = new ObservableCollection<FeedViewModel>();
        #endregion

        #region helper
         private void ClickedFeedView(FeedViewModel feedViewModel)
         {
            _eventAggregator.GetEvent<WantUriEvent>().Publish(feedViewModel.Link);
         }

        private void ShouldUpdateFeedList(bool flag)
        {
            if(flag)
                UpdateFeedList(_lastFeedUri);
        }
        
        private void UpdateFeedList(Source source)
        {
            UpdateFeedList(source.FeedUri);
        }

        private void UpdateFeedList(ICollection<Source> sources)
        {
            foreach (var source in sources)
            {
               UpdateFeedList(source); 
            }
        }

        private void UpdateFeedList(Uri uri)
        {
                var awaiter = _feedService.GetTaskAllFeedsFromUrlAsync(uri).GetAwaiter();
                awaiter.OnCompleted(() =>
                {
                    _logger.Log("Update the feeds with the url " + uri.OriginalString, Category.Info, Priority.Medium);
                    //TODO: nicht cleanen
                    AllFeeds.Clear();
                    _lastFeedUri = uri;

                    foreach (var oneFeed in awaiter.GetResult())
                    {
                       AllFeeds.Add(oneFeed); 
                    }
                });
        }
        #endregion
    }
}
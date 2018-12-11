using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
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

            AddArchiveFeedDelegateCommand = new DelegateCommand<FeedViewModel>(AddArchiveFeed);

            eventAggregator.GetEvent<FetchDataEvent>().Subscribe(ShouldUpdateFeedList);
            eventAggregator.GetEvent<WantFeedEvent>().Subscribe(UpdateFeedListWithClear);
            eventAggregator.GetEvent<WantAllFeedsEvent>().Subscribe(UpdateFeedListWithClear);
        }

	    public DelegateCommand<FeedViewModel> AddArchiveFeedDelegateCommand { get; }

        #region attributes
        /// <summary>
        /// all feeds that will be shown from a source 
        /// </summary>
        public ICollection<FeedViewModel> AllFeeds { get; } = new ObservableCollection<FeedViewModel>();
        #endregion

        #region helper
        private void AddArchiveFeed(FeedViewModel feedViewModel)
        {
            _eventAggregator.GetEvent<NewArchiveFeedEvent>().Publish(feedViewModel);
        }

        private void ClickedFeedView(FeedViewModel feedViewModel)
        {
            _eventAggregator.GetEvent<WantUriEvent>().Publish(feedViewModel.Link);
        }

        //TODO: Nicht letzte Source updaten sondern auch eventuell alle
        private void ShouldUpdateFeedList(bool flag)
        {
            if(flag)
                UpdateFeedList(_lastFeedUri);
        }
        
        private void UpdateFeedListWithClear(Source source)
        {
            AllFeeds.Clear();
            UpdateFeedList(source.FeedUri);
        }

        private void UpdateFeedListWithClear(ICollection<Source> sources)
        {
            AllFeeds.Clear();
            foreach (var source in sources)
            {
               UpdateFeedList(source.FeedUri); 
            }

            OrderList();
        }

        private void UpdateFeedList(Uri uri)
        {
                var awaiter = _feedService.GetTaskAllFeedsFromUrlAsync(uri).GetAwaiter();
                awaiter.OnCompleted(() =>
                {
                    _logger.Log("Update the feeds with the url " + uri.OriginalString, Category.Info, Priority.Medium);
                    _lastFeedUri = uri;

                    foreach (var oneFeed in awaiter.GetResult())
                    {
                       AllFeeds.Add(oneFeed); 
                    }
                });
        }

        private void OrderList()
        {
            /*
            var feedsSortedByDate = from feed in AllFeeds
                                    orderby feed.PublishedDate ascending
                                    select feed;

            AllFeeds.Clear();

            foreach (var feed in feedsSortedByDate)
            {
               AllFeeds.Add(feed); 
            }
            */
        }
        #endregion
    }
}
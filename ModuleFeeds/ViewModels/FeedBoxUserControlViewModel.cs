using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
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
        private Source _lastSource;
        private readonly IFeedService _feedService = new FeedServiceImpl();
        public DelegateCommand<FeedViewModel> ChangeFeedCommand { get; }

        public FeedBoxUserControlViewModel(IEventAggregator eventAggregator)
        {
            ChangeFeedCommand = new DelegateCommand<FeedViewModel>(ClickedFeedView);
            _eventAggregator = eventAggregator;

            AddArchiveFeedDelegateCommand = new DelegateCommand<FeedViewModel>(AddArchiveFeed);

            eventAggregator.GetEvent<FetchDataEvent>().Subscribe(ShouldUpdateFeedList);
            eventAggregator.GetEvent<WantFeedEvent>().Subscribe(UpdateFeedListWithClear);
            eventAggregator.GetEvent<WantAllFeedsEvent>().Subscribe(UpdateFeedListWithClear);
        }

	    public DelegateCommand<FeedViewModel> AddArchiveFeedDelegateCommand { get; }

        #region attributes

        private readonly ICollection<FeedViewModel> _allFeeds = new ObservableCollection<FeedViewModel>();
        /// <summary>
        /// all feeds that will be shown from a source 
        /// </summary>
        public ICollection<FeedViewModel> AllFeeds
        {
            get
            {
                var orderedFeedList = _allFeeds.OrderByDescending(x => x.PublishedDate).ToList();
                _allFeeds.Clear();

                foreach (var feed in orderedFeedList)
                {
                   _allFeeds.Add(feed); 
                }

                return _allFeeds;
            }
        }

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

        private void ShouldUpdateFeedList(bool flag)
        {
            if(flag)
                UpdateFeedList(_lastSource);
        }
        
        private void UpdateFeedListWithClear(Source source)
        {
            AllFeeds.Clear();
            UpdateFeedList(source);
        }

        private void UpdateFeedListWithClear(ICollection<Source> sources)
        {
            AllFeeds.Clear();
            UpdateFeedList(sources);
        }

        private void UpdateFeedList(Source source)
        {
            var awaiter = _feedService.GetTaskAllFeedsFromUrlAsync(source.FeedUri).GetAwaiter();
            awaiter.OnCompleted(() =>
            {
                _logger.Log("Update the feeds with the url " + source.FeedUri.OriginalString, Category.Info,
                    Priority.Medium);
                _lastSource = source;

                foreach (var oneFeed in awaiter.GetResult())
                {
                    AllFeeds.Add(oneFeed);
                }
            });
        }

        private void UpdateFeedList(ICollection<Source> sources)
        {
            foreach (var source in sources)
            {
                UpdateFeedList(source);
            }
        }
        #endregion
    }
}
﻿using System;
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
using MaterialDesignThemes.Wpf;
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
        private readonly ICollection<Source> _lastSources = new ObservableCollection<Source>();
        private readonly IFeedService _feedService = new FeedServiceImpl();
        public DelegateCommand<FeedViewModel> ChangeFeedCommand { get; }

        public FeedBoxUserControlViewModel(IEventAggregator eventAggregator)
        {
            ChangeFeedCommand = new DelegateCommand<FeedViewModel>(ClickedFeedView);
            _eventAggregator = eventAggregator;

            AddArchiveFeedDelegateCommand = new DelegateCommand<FeedViewModel>(AddArchiveFeed);
            SearchCommand = new DelegateCommand(SearchWithTerm);
            CleanFilterCommand = new DelegateCommand(Reset);

            eventAggregator.GetEvent<FetchDataEvent>().Subscribe(ShouldUpdateFeedList);
            eventAggregator.GetEvent<WantFeedEvent>().Subscribe(UpdateFeedListWithClear);
            eventAggregator.GetEvent<WantAllFeedsEvent>().Subscribe(UpdateFeedListWithClear);

            _allFeeds = new ObservableCollection<FeedViewModel>();
            _allFeeds.CollectionChanged += (a, e) =>
            {
                RaisePropertyChanged(nameof(IsEmpty));
                RaisePropertyChanged(nameof(IsNotEmpty));
            };

            var myMessageQueue = new SnackbarMessageQueue(TimeSpan.FromMilliseconds(5000));
            AddArchiveMessageQueue = myMessageQueue;
        }

        public DelegateCommand<FeedViewModel> AddArchiveFeedDelegateCommand { get; }
        public DelegateCommand SearchCommand { get; }
        public DelegateCommand CleanFilterCommand { get; }

        #region attributes

        private string _header;

        public string Header
        {
            get => _header;
            set => SetProperty(ref _header, value);
        }

        private string _searchTerm;

        public string SearchTerm
        {
            get => _searchTerm;
            set => SetProperty(ref _searchTerm, value);
        }

        public bool IsEmpty => _allFeeds.Count == 0;
        public bool IsNotEmpty => !IsEmpty;
        
        private readonly ObservableCollection<FeedViewModel> _allFeeds;

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

        private SnackbarMessageQueue _addArchiveMessageQueue;

        public SnackbarMessageQueue AddArchiveMessageQueue
        {
            get => _addArchiveMessageQueue;
            set => SetProperty(ref _addArchiveMessageQueue, value);
        }

    #region helper
        private void Reset()
        {
            SearchTerm = string.Empty;

            if (_lastSources != null && _lastSources.Count > 0)
                UpdateFeedListWithClear(_lastSources);
        }

        private void SearchWithTerm()
        {

            if (string.IsNullOrWhiteSpace(SearchTerm))
            {
                Reset();
                return;
            }
            
            var foundFeeds = from feeds in _allFeeds
                where feeds.Title.Contains(SearchTerm)  || feeds.ShortDescription.Contains(SearchTerm)
                select feeds;

            var list = new ObservableCollection<FeedViewModel>(foundFeeds);

            _allFeeds.Clear();

            foreach (var oneFeed in list)
            {
                _logger.Log("Title: " + oneFeed.Title, Category.Info, Priority.Medium);
                _allFeeds.Add(oneFeed);
            }
        }

        private void AddArchiveFeed(FeedViewModel feedViewModel)
        {
            AddArchiveMessageQueue.Enqueue("Add \"" + feedViewModel.Title + "\" to the archive");
            _eventAggregator.GetEvent<NewArchiveFeedEvent>().Publish(feedViewModel);
        }

        private void ClickedFeedView(FeedViewModel feedViewModel)
        {
            _eventAggregator.GetEvent<WantUriEvent>().Publish(feedViewModel.Link);
        }

        private void ShouldUpdateFeedList(bool flag)
        {
            if(flag)
                UpdateFeedListWithClear(_lastSources);
        }
        
        private void UpdateFeedListWithClear(Source source)
        {
            AllFeeds.Clear();
            Header = "Feeds for " + source.Name;
            UpdateFeedList(source);

            _lastSources.Clear();
            _lastSources.Add(source);
        }

        private void UpdateFeedListWithClear(ICollection<Source> sources)
        {
            ICollection<Source> cpSource = new ObservableCollection<Source>(sources);
            AllFeeds.Clear();

            if (cpSource.Count > 1)
            {
                Header = "Feeds for all Sources";
            }
            else if (cpSource.Count == 1)
            {
                Header = "Feeds for " + sources.ElementAt(0).Name;
            }
            else
            {
                Header = "No Feeds Available";
            }

            UpdateFeedList(cpSource);

            _lastSources.Clear();
            foreach (var source in cpSource)
            {
               _lastSources.Add(source); 
            }
        }

        private void UpdateFeedList(Source source)
        {
            var awaiter = _feedService.GetTaskAllFeedsFromUrlAsync(source.FeedUri).GetAwaiter();
            awaiter.OnCompleted(() =>
            {
                _logger.Log("Update the feeds with the url " + source.FeedUri.OriginalString, Category.Info,
                    Priority.Medium);

                foreach (var oneFeed in awaiter.GetResult())
                {
                    AllFeeds.Add(oneFeed);
                }

                _eventAggregator.GetEvent<FeedsLoadedEvent>().Publish(true);
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
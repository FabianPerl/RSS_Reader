using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

namespace ModuleFeeds.ViewModels
{
    /// <summary>
    /// Viewmodel for the feeds
    /// </summary>
    public class FeedBoxUserControlViewModel : BindableBase
    {
        private readonly ILoggerFacade _logger = ProjectLogger.GetLogger;
        private readonly IEventAggregator _eventAggregator;
        private readonly ICollection<Source> _lastSources = new ObservableCollection<Source>();
        private readonly IFeedService _feedService = new FeedServiceImpl();

        public FeedBoxUserControlViewModel(IEventAggregator eventAggregator)
        {
            _logger.Log("Initialize the viewmodel for the feeds", Category.Info, Priority.Medium);
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

        #region delegates
        /// <summary>
        /// Handles the interaction that the user wants to see the uri from the selected feed
        /// </summary>
        public DelegateCommand<FeedViewModel> ChangeFeedCommand { get; }

        /// <summary>
        /// Handles the interaction that the user wants to add the selected feed to the archive
        /// </summary>
        public DelegateCommand<FeedViewModel> AddArchiveFeedDelegateCommand { get; }

        /// <summary>
        /// Handles the interaction that the user wants to filter the feeds with the search term
        /// </summary>
        public DelegateCommand SearchCommand { get; }

        /// <summary>
        /// Handles the interaction that the user wants to clean the search term
        /// </summary>
        public DelegateCommand CleanFilterCommand { get; }
        #endregion

        #region attributes
        private SnackbarMessageQueue _addArchiveMessageQueue;

        /// <summary>
        /// Gets and Sets the snackbar message queue, that is shown when an feed was added to the Archive 
        /// </summary>
        public SnackbarMessageQueue AddArchiveMessageQueue
        {
            get => _addArchiveMessageQueue;
            set => SetProperty(ref _addArchiveMessageQueue, value);
        }

        private string _header;

        /// <summary>
        /// Gets and Sets the title for the shown feeds
        /// </summary>
        public string Header
        {
            get => _header;
            set => SetProperty(ref _header, value);
        }

        private string _searchTerm;

        /// <summary>
        /// Gets and Sets the search term, after which the feeds should be filtered
        /// </summary>
        public string SearchTerm
        {
            get => _searchTerm;
            set => SetProperty(ref _searchTerm, value);
        }

        /// <summary>
        /// Returns true if and only if the feed list is empty
        /// </summary>
        public bool IsEmpty => _allFeeds.Count == 0;

        /// <summary>
        /// Returns true if and only if the feed list is not empty
        /// </summary>
        public bool IsNotEmpty => !IsEmpty;
        
        private readonly ObservableCollection<FeedViewModel> _allFeeds;

        /// <summary>
        /// All feeds that will be shown from a source, ordered by the published date
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
        /// <summary>
        /// Sets the search term to empty and updates the feed list with the last known source when possible
        /// </summary>
        private void Reset()
        {
            _logger.Log("Reset the search term for feeds", Category.Info, Priority.Medium);
            SearchTerm = string.Empty;

            if (_lastSources != null && _lastSources.Count > 0)
                UpdateFeedListWithClear(_lastSources);
        }

        /// <summary>
        /// Filters the feed list with the search term when possible or uses the last known source and updates the feeds
        /// </summary>
        private void SearchWithTerm()
        {
            _logger.Log("Search with the filter " + SearchTerm + " on the feeds", Category.Info, Priority.Medium);
            if (string.IsNullOrWhiteSpace(SearchTerm))
            {
                Reset();
                return;
            }
            
            var foundFeeds = from feeds in _allFeeds
                where feeds.Title.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase)  || 
                      feeds.ShortDescription.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase)
                select feeds;

            var list = new ObservableCollection<FeedViewModel>(foundFeeds);

            _allFeeds.Clear();

            foreach (var oneFeed in list)
            {
                _allFeeds.Add(oneFeed);
            }

            _logger.Log("Found " + list.Count + " with search term " + SearchTerm, Category.Info, Priority.Medium);
        }

        /// <summary>
        /// Adds a feed to the archive
        /// </summary>
        /// <param name="feedViewModel">The feed that should be added</param>
        private void AddArchiveFeed(FeedViewModel feedViewModel)
        {
            AddArchiveMessageQueue.Enqueue("Add \"" + feedViewModel.Title + "\" to the archive");
            _eventAggregator.GetEvent<NewArchiveFeedEvent>().Publish(feedViewModel);
        }

        /// <summary>
        /// Publishes an event that the user wants to see the uri for a feed
        /// </summary>
        /// <param name="feedViewModel">The feed for which the uri should be shown</param>
        private void ClickedFeedView(FeedViewModel feedViewModel)
        {
            _logger.Log("Publish event that the user wants to see the uri for the feed " + feedViewModel.Title, Category.Info, Priority.Medium);
            _eventAggregator.GetEvent<WantUriEvent>().Publish(feedViewModel.Link);
        }

        /// <summary>
        /// Updates the list the last known source
        /// </summary>
        /// <param name="flag">Updates the list only if and only if the flag is true</param>
        private void ShouldUpdateFeedList(bool flag)
        {
            _logger.Log("Received event that the user wants to update the last known source", Category.Info, Priority.Medium);
            if(flag)
                UpdateFeedListWithClear(_lastSources);
        }
        
        /// <summary>
        /// Cleans the feed list and updates the list with the source's feeds
        /// </summary>
        /// <param name="source">The source for the feeds</param>
        private void UpdateFeedListWithClear(Source source)
        {
            _logger.Log("Update the feed list with source " + source.Name, Category.Info, Priority.Medium);
            AllFeeds.Clear();
            Header = "Feeds for " + source.Name;
            UpdateFeedList(source);

            _lastSources.Clear();
            _lastSources.Add(source);
        }

        /// <summary>
        /// Cleans the feed list and updates the list with the sources feeds
        /// </summary>
        /// <param name="sources">The sources for the feeds</param>
        private void UpdateFeedListWithClear(ICollection<Source> sources)
        {
            _logger.Log("Update the feed list with all sources", Category.Info, Priority.Medium);
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

        /// <summary>
        /// Fetches the feeds for the source and publishes an event that the feeds are loaded
        /// </summary>
        /// <param name="source"></param>
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

        /// <summary>
        /// Fetches the feeds for the sources
        /// </summary>
        /// <param name="sources"></param>
        private void UpdateFeedList(ICollection<Source> sources)
        {
            foreach (var source in sources)
            {
                UpdateFeedList(source);
            }
        }
        #endregion
    }

    /// <summary>
    /// This class overrides the String method to filter after a search term with a self defined string comparison
    /// Source: https://stackoverflow.com/questions/444798/case-insensitive-containsstring
    /// </summary>
    public static class StringExtensions
    {
        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source?.IndexOf(toCheck, comp) >= 0;
        }
    }
}
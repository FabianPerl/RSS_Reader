using System;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Infrastructure.Constants;
using Infrastructure.Events;
using Infrastructure.Services;
using Infrastructure.ViewModels;
using Prism.Events;
using Prism.Logging;

namespace ModuleArchiveFeeds.ViewModels
{
    /// <summary>
    /// Viewmodel for the archived feeds
    /// </summary>
    public class ArchiveFeedBoxUserControlViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly ILoggerFacade _logger = ProjectLogger.GetLogger;
        private readonly IRssStore _rssStore;

        public ArchiveFeedBoxUserControlViewModel(IEventAggregator eventAggregator, IRssStore rssStore)
        {
            _logger.Log("Initialize the viewmodel for the archive", Category.Info, Priority.Medium);
            AllArchivedFeeds = rssStore.LoadAllArchiveFeeds();
            _eventAggregator = eventAggregator;
            _rssStore = rssStore;

            ChangeFeedCommand = new DelegateCommand<FeedViewModel>(ClickedArchiveFeed);
            RemoveFromArchiveCommand = new DelegateCommand<FeedViewModel>(RemoveFromArchiveFeed);
            SearchCommand = new DelegateCommand(SearchWithTerm);
            CleanFilterCommand = new DelegateCommand(Reset);

            eventAggregator.GetEvent<NewArchiveFeedEvent>().Subscribe(AddNewArchiveFeed);
            eventAggregator.GetEvent<RemoveArchiveFeedEvent>().Subscribe(RemoveFromArchiveFeed);

        }

        #region delegates
        /// <summary>
        /// Handles the interaction that the user wants to see the uri from the selected feed
        /// </summary>
        public DelegateCommand<FeedViewModel> ChangeFeedCommand { get; }

        /// <summary>
        /// Handles the interaction to remove the selected archived feed from the list
        /// </summary>
        public DelegateCommand<FeedViewModel> RemoveFromArchiveCommand { get; }

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

        private ICollection<FeedViewModel> _allArchivedFeeds;
        private ICollection<FeedViewModel> _cpAllArchivedFeeds;

        /// <summary>
        /// Gets the archived feeds, sorted after the published date from each Feed
        /// </summary>
        public ICollection<FeedViewModel> AllArchivedFeeds
        {
            get
            {
                _logger.Log("Get all archived feeds ordered by published date", Category.Info, Priority.Medium);
                var orderedFeedList = _allArchivedFeeds?.OrderByDescending(x => x.PublishedDate).ToList();
                _allArchivedFeeds?.Clear();

                if (orderedFeedList != null)
                {
                    foreach (var feed in orderedFeedList)
                    {
                        _allArchivedFeeds.Add(feed);
                    }
                }

                return _allArchivedFeeds;
            }
            private set => SetProperty(ref _allArchivedFeeds, value);
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
        #endregion

        #region helper
        /// <summary>
        /// Resets the search term with an empty string and updates the feeds 
        /// </summary>
        private void Reset()
        {
            _logger.Log("Reset the Search term in Archive", Category.Info, Priority.Medium);
            SearchTerm = string.Empty;

            if (_cpAllArchivedFeeds != null)
            {
                _allArchivedFeeds.Clear();

                foreach (var feed in _cpAllArchivedFeeds)
                {
                   _allArchivedFeeds.Add(feed); 
                }

                _cpAllArchivedFeeds = null;
            }
        }

        /// <summary>
        /// Filters the feeds with the search term and updates the list
        /// </summary>
        private void SearchWithTerm()
        {
            _logger.Log("Search with term: " + SearchTerm, Category.Info, Priority.Medium);

            if (string.IsNullOrWhiteSpace(SearchTerm))
            {
                Reset();
                return;
            }

            var foundFeeds = from feeds in _allArchivedFeeds
                where feeds.Title.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) || 
                      feeds.ShortDescription.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase)
                select feeds;

            var list = new ObservableCollection<FeedViewModel>(foundFeeds);

            _cpAllArchivedFeeds = new ObservableCollection<FeedViewModel>(_allArchivedFeeds);
            _allArchivedFeeds.Clear();

            foreach (var oneFeed in list)
            {
                _allArchivedFeeds.Add(oneFeed);
            }
            _logger.Log("Found " +  list.Count + " with Search term " + SearchTerm, Category.Info, Priority.Medium);
        }

        /// <summary>
        /// Publishes an event that the user wants to see the uri from the selected feed
        /// </summary>
        /// <param name="feedViewModel">The feed that the user wants to see</param>
        private void ClickedArchiveFeed(FeedViewModel feedViewModel)
        {
            _logger.Log("Publish UriEvent with Link: " + feedViewModel.Link + " from the Archive", Category.Info, Priority.Medium);
            _eventAggregator.GetEvent<WantUriEvent>().Publish(feedViewModel.Link);
        }

        /// <summary>
        ///  Adds the feed to the archive only if and only if the feed does not exist yet in the list
        /// </summary>
        /// <param name="feedViewModel">The feed that should be added</param>
        private void AddNewArchiveFeed(FeedViewModel feedViewModel)
        {
            _logger.Log("Add Feed " + feedViewModel.Title + " to the Archive", Category.Info, Priority.Medium);

            if (!AllArchivedFeeds.Contains(feedViewModel))
            {
                AllArchivedFeeds.Add(feedViewModel);
                _rssStore.SafeAllArchiveFeeds(AllArchivedFeeds);
            }
        }

        /// <summary>
        /// Removes the feed from the archive
        /// </summary>
        /// <param name="feedViewModel">The feed that should be removed</param>
        private void RemoveFromArchiveFeed(FeedViewModel feedViewModel)
        {
            _logger.Log("Remove Feed " + feedViewModel.Title + " from the Archive", Category.Info, Priority.Medium);
            AllArchivedFeeds.Remove(feedViewModel);
            _rssStore.SafeAllArchiveFeeds(AllArchivedFeeds);
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

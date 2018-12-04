using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using Prism.Logging;
using RSSReader.Models;
using RSSReader.Services;

namespace RSSReader.ViewModels
{
    public class FeedBoxUserControlViewModel : BindableBase
    {
        private ObservableCollection<FeedViewModel> _allFeeds;
        private readonly DebugLogger _debugLogger = new DebugLogger();
        private Uri _currentUri;
        private readonly IFeedService _feedService = new FeedServiceImpl();
        private Source _currentSource;
        private readonly SourceList _sourceList = SourceList.GetInstance;

        public FeedBoxUserControlViewModel()
        {
            _currentUri = new Uri("https://www.heise.de");
            var newSource = new Source
            {
                FeedUri = new Uri("https://www.heise.de/newsticker/heise-atom.xml"),
                Name = "Heise online",
                Category = "Technik"
            };

            CurrentSource = newSource;
            _sourceList.Add(newSource);
        }

        /// <summary>
        /// all feeds that will be shown from a source 
        /// </summary>
        public ObservableCollection<FeedViewModel> AllFeeds
        {
            get => _allFeeds;
            set => SetProperty(ref _allFeeds, value);
        }

        public Source CurrentSource
        {
            get => _currentSource;
            set
            {
                SetProperty(ref _currentSource, value);
                UpdateFeedList(value.FeedUri);
            }
        }

        public void UpdateFeedList()
        {
            UpdateFeedList(_currentSource.FeedUri);
        }

        public void UpdateFeedList(Uri uri)
        {
                var awaiter = _feedService.GetTaskAllFeedsFromUrlAsync(uri).GetAwaiter();
                awaiter.OnCompleted(() =>
                {
                    _debugLogger.Log("Update the feeds with the url " + uri.OriginalString, Category.Info, Priority.Medium);
                    AllFeeds = awaiter.GetResult();
                });
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
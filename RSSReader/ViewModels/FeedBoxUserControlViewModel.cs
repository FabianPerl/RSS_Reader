using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using Prism.Commands;
using Prism.Logging;
using RSSReader.Models;
using RSSReader.Services;

namespace RSSReader.ViewModels
{
    public class FeedBoxUserControlViewModel : BindableBase
    {
        private readonly DebugLogger _debugLogger = new DebugLogger();
        private Uri _currentUri;
        private readonly IFeedService _feedService = new FeedServiceImpl();
       
        public FeedBoxUserControlViewModel()
        {
            _currentUri = new Uri("https://www.heise.de");
            UpdateFeedList(new Uri("https://www.heise.de/newsticker/heise-atom.xml"));
        }

        /// <summary>
        /// all feeds that will be shown from a source 
        /// </summary>
        public ObservableCollection<FeedViewModel> AllFeeds { get; set; } = SingletonList<FeedViewModel>.GetInstance;

        private void UpdateFeedList(Uri uri)
        {
                var awaiter = _feedService.GetTaskAllFeedsFromUrlAsync(uri).GetAwaiter();
                awaiter.OnCompleted(() =>
                {
                    _debugLogger.Log("Update the feeds with the url " + uri.OriginalString, Category.Info, Priority.Medium);
                    AllFeeds.Clear();
                    AllFeeds.AddRange(awaiter.GetResult());
                });
        }

        /// <summary>
        /// set and get the current uri that is shown in the window
        /// </summary>
        public Uri CurrentUri
        {
            get => _currentUri;
            set
            {
                SetProperty(ref _currentUri, value);
                UpdateFeedList(value);
            }
        }
    }
}
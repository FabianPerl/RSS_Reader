using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Infrastructure.Services;
using Infrastructure.ViewModels;
using Prism.Commands;
using Prism.Logging;
using Prism.Mvvm;

namespace ModuleFeeds.ViewModels
{
    public class FeedBoxUserControlViewModel : BindableBase
    {
        private readonly DebugLogger _debugLogger = new DebugLogger();
        private readonly IFeedService _feedService = new FeedServiceImpl();
        public DelegateCommand<FeedViewModel> ChangeFeedCommand { get; }
       
        public FeedBoxUserControlViewModel()
        {
            UpdateFeedList(new Uri("https://www.heise.de/newsticker/heise-atom.xml"));
            ChangeFeedCommand = new DelegateCommand<FeedViewModel>(ClickedFeedView);
        }

        /// <summary>
        /// all feeds that will be shown from a source 
        /// </summary>
        public ICollection<FeedViewModel> AllFeeds { get; set; } = new ObservableCollection<FeedViewModel>();

        private void ClickedFeedView(FeedViewModel feedViewModel)
        {
            //CurrentUri = feedViewModel.Link;
            //Stoest den Browser an die URI zu wechseln
        }

        private void UpdateFeedList(Uri uri)
        {
                var awaiter = _feedService.GetTaskAllFeedsFromUrlAsync(uri).GetAwaiter();
                awaiter.OnCompleted(() =>
                {
                    _debugLogger.Log("Update the feeds with the url " + uri.OriginalString, Category.Info, Priority.Medium);
                    AllFeeds.Clear();

                    foreach (var oneFeed in awaiter.GetResult())
                    {
                       AllFeeds.Add(oneFeed); 
                    }
                });
        }
    }
}
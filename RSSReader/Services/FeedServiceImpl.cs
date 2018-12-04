using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeHollow.FeedReader;
using Prism.Logging;
using RSSReader.ViewModels;

namespace RSSReader.Services
{
    public class FeedServiceImpl : IFeedService
    {
        private readonly DebugLogger _debugLogger = new DebugLogger();

        public Task<ObservableCollection<FeedViewModel>> GetTaskAllFeedsFromUrlAsync(Uri feedUri)
        {
            if (feedUri == null)
            {
                throw new ArgumentNullException(nameof(feedUri));
            }

            return GetTaskAllFeedsFromUrlAsyncInternal(feedUri);
        }

        private async Task<ObservableCollection<FeedViewModel>> GetTaskAllFeedsFromUrlAsyncInternal(Uri feedUri)
        {
            //TODO: Eventuell auf Syndication umsteigen..
            _debugLogger.Log("Get all the Feeds for URI " + feedUri, Category.Info, Priority.Medium);
            var allFeedsList = new ObservableCollection<FeedViewModel>();

            var feed = await FeedReader.ReadAsync(feedUri.OriginalString);
            foreach (var item in feed.Items)
            {
                var feedViewModel = new FeedViewModel
                {
                    Title = item.Title?.Trim(),
                    Author = item.Author?.Trim(),
                    Link = new Uri(item.Link),
                    PublishedDate = item.PublishingDate ?? new DateTime(),
                    ShortDescription = item.Description?.Trim()
                };
                
                allFeedsList.Add(feedViewModel);
            }
            return allFeedsList;
        }
    }
}

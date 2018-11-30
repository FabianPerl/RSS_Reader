using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CodeHollow.FeedReader;
using Prism.Logging;

namespace RSSReader.ViewModels
{
	public class FeedBoxUserControlViewModel : BindableBase
	{
	    private List<FeedViewModel> _allFeeds;
        private readonly DebugLogger _debugLogger = new DebugLogger();
	    private Uri _currentUri;

	    public List<FeedViewModel> AllFeeds
	    {
	        get => _allFeeds; 
	        set => SetProperty(ref _allFeeds, value); 
	    }

	    public Uri CurrentUri
	    {
	        get => _currentUri;
	        set => SetProperty(ref _currentUri, value);
	    }

	    public async Task<List<FeedViewModel>> GetAllFeedsFromUrlAsync(string feedUri)
	    {
            _debugLogger.Log("Get all the Feeds for URI " + feedUri, Category.Info, Priority.Medium);

	        var allFeedsList = new List<FeedViewModel>();
	        try
	        {
	            var feed = await FeedReader.ReadAsync(feedUri);
	            foreach (var item in feed.Items)
	            {
	                var feedViewModel = new FeedViewModel();
	                feedViewModel.Title = item.Title?.Trim();
	                feedViewModel.Author = item.Author?.Trim();
	                feedViewModel.Link = new Uri(item.Link);
	                feedViewModel.PublishedDate = item.PublishingDate.HasValue ? item.PublishingDate.Value : new DateTime();
	                feedViewModel.ShortDescription = item.Description?.Trim();
	                allFeedsList.Add(feedViewModel);
	            }
	            AllFeeds = allFeedsList;
	        }
	        catch (Exception e)
	        {
                Console.WriteLine("Error!");
                Console.WriteLine(e.Message);
	        }

	        return allFeedsList;
	    }
    }
} 
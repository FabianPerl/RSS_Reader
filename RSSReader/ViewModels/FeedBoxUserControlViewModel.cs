﻿using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CodeHollow.FeedReader;
using Prism.Logging;
using RSSReader.Models;

namespace RSSReader.ViewModels
{
	public class FeedBoxUserControlViewModel : BindableBase
	{
	    private List<FeedViewModel> _allFeeds;
        private readonly DebugLogger _debugLogger = new DebugLogger();
	    private Uri _currentUri = new Uri("https://www.heise.de");

	    public FeedBoxUserControlViewModel()
	    {
	        Source a = new Source();
	        a.Name = "Heise Online";
	        a.Category = "IT";
	        a.FeedUri = new Uri("https://www.heise.de");
	        AllSources.Add(a);

	        Source b = new Source();
	        b.Name = "Golem";
	        b.Category = "IT";
	        b.FeedUri = new Uri("https://www.golem.de");
	        AllSources.Add(b);

	        Source c = new Source();
	        c.Name = "Reddit";
	        c.Category = "IT";
	        c.FeedUri = new Uri("https://www.reddit.com");
	        AllSources.Add(c);
    }

	    public List<FeedViewModel> AllFeeds
	    {
	        get => _allFeeds; 
	        set => SetProperty(ref _allFeeds, value); 
	    }

	    public Uri CurrentUri
	    {
	        get => _currentUri;
	        set
	        {
	            SetProperty(ref _currentUri, value);
	            var awaiter = GetTaskAllFeedsFromUrlAsyncInternal(value).GetAwaiter();
	            awaiter.OnCompleted(() => SetProperty(ref _allFeeds, awaiter.GetResult()));
	        }
	    }

	    public Task<List<FeedViewModel>> GetTaskAllFeedsFromUrlAsyncInternal(Uri feedUri)
	    {
	        if (feedUri == null)
	        {
	            throw new ArgumentNullException("feedUri");
	        }

	        return GetTaskAllFeedsFromUrlAsync(feedUri);
	    }

	    private async Task<List<FeedViewModel>> GetTaskAllFeedsFromUrlAsync(Uri feedUri)
	    {
            //TODO: Eventuell auf Syndication umsteigen..
            _debugLogger.Log("Get all the Feeds for URI " + feedUri, Category.Info, Priority.Medium);
	        var allFeedsList = new List<FeedViewModel>();

            var feed = await FeedReader.ReadAsync(feedUri.AbsolutePath);
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

	        return allFeedsList;
	    }
      
        public List<Source> AllSources { get; } = new List<Source>();

        public void AddSource(Source source)
        {
            AllSources.Add(source);
        }
        public bool RemoveSource(Source source)
        {
            return AllSources.Remove(source);
        }
}
} 
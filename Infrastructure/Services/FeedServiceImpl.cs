using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Infrastructure.ViewModels;
using System.ServiceModel.Syndication;
using System.Xml;
using Prism.Logging;

namespace Infrastructure.Services
{
    public class FeedServiceImpl : IFeedService
    {
        private readonly DebugLogger _debugLogger = new DebugLogger();

        public async Task<ICollection<FeedViewModel>> GetTaskAllFeedsFromUrlAsync(Uri feedUri)
        {
            if (feedUri == null)
            {
                throw new ArgumentNullException(nameof(feedUri));
            }

            return await GetTaskAllFeedsFromUrlAsyncInternal2(feedUri);
        }

        private Task<ICollection<FeedViewModel>> GetTaskAllFeedsFromUrlAsyncInternal2(Uri feedUri)
        {
            return Task.Run(() =>
            {
                ICollection<FeedViewModel> allFeedsList = new ObservableCollection<FeedViewModel>();
                using (var xmlReader = XmlReader.Create(feedUri.OriginalString))
                {
                        var readSyndicationFeed = SyndicationFeed.Load(xmlReader);

                        foreach (var item in readSyndicationFeed.Items)
                        {
                            ICollection<string> authors = new List<string>(); 
                            foreach (var element in item.Authors)
                            {
                                authors.Add(element.Name);    
                            }

                            var newFeedViewModel = new FeedViewModel
                            {
                                PublishedDate = item.PublishDate,
                                Link = item.Links.Count > 0 ? item.Links[0].Uri : null,
                                Title = item.Title?.Text.Trim(),
                                ShortDescription = item.Summary?.Text.Trim(),
                                ImageUrl = readSyndicationFeed.ImageUrl?.OriginalString.Trim(),
                                Authors = authors
                            };

                            allFeedsList.Add(newFeedViewModel);
                        }
                }

                return allFeedsList;
            });
        }
    }
}

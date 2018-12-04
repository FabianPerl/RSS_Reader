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
    internal interface IFeedService
    {
        Task<ObservableCollection<FeedViewModel>> GetTaskAllFeedsFromUrlAsync(Uri feedUri);
    }
}

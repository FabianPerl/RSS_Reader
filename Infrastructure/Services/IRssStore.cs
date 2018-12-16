using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Models;
using Infrastructure.ViewModels;

namespace Infrastructure.Services
{
    public interface IRssStore
    {
        //Sources
        ICollection<Source> GetAllSources();
        void DeleteAllSources();
        void SafeAllSources(ICollection<Source> allSources);


        //ArchiveFeeds
        void SafeAllArchiveFeeds(ICollection<FeedViewModel> allFeedViewModels);
        ICollection<FeedViewModel> LoadAllArchiveFeeds();
    }
}

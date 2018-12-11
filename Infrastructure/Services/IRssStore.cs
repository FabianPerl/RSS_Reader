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
        ICollection<Source> GetAllSources();
        void DeleteAllSources();
        void SafeAllSources(ICollection<Source> allSources);
        void SafeAllArchives(ICollection<FeedViewModel> allFeedViewModels);
    }
}

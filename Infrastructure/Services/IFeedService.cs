using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.ViewModels;

namespace Infrastructure.Services
{
    public interface IFeedService
    {
        Task<ICollection<FeedViewModel>> GetTaskAllFeedsFromUrlAsync(Uri feedUri);
    }
}

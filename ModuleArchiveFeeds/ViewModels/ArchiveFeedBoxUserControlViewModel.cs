using Prism.Commands;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Infrastructure.Constants;
using Infrastructure.Events;
using Infrastructure.ViewModels;
using Prism.Events;
using Prism.Logging;

namespace ModuleArchiveFeeds.ViewModels
{
    public class ArchiveFeedBoxUserControlViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly ILoggerFacade _logger = ProjectLogger.GetLogger;

        public ArchiveFeedBoxUserControlViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            ChangeFeedCommand = new DelegateCommand<FeedViewModel>(ClickedArchiveFeed);
            eventAggregator.GetEvent<NewArchiveFeedEvent>().Subscribe(AddNewArchiveFeed);
            eventAggregator.GetEvent<RemoveArchiveFeedEvent>().Subscribe(RemoveFromArchiveFeed);
        }

        #region delegates
        public DelegateCommand<FeedViewModel> ChangeFeedCommand { get; }
        #endregion

        #region attributes
        public ICollection<FeedViewModel> AllArchivedFeeds { get; } = new ObservableCollection<FeedViewModel>();
        #endregion

        #region helper
        private void ClickedArchiveFeed(FeedViewModel feedViewModel)
        {
            _eventAggregator.GetEvent<WantUriEvent>().Publish(feedViewModel.Link);
        }

        private void AddNewArchiveFeed(FeedViewModel feedViewModel)
        {
            _logger.Log("Add Feed " + feedViewModel.Title + " to the Archive", Category.Info, Priority.Medium);
            if(!AllArchivedFeeds.Contains(feedViewModel))
                AllArchivedFeeds.Add(feedViewModel);
        }

        private void RemoveFromArchiveFeed(FeedViewModel feedViewModel)
        {
            _logger.Log("Remove Feed " + feedViewModel.Title + " from the Archive", Category.Info, Priority.Medium);
            foreach (var archiveFeed in AllArchivedFeeds)
            {
                if (archiveFeed.Equals(feedViewModel))
                {
                   AllArchivedFeeds.Remove(feedViewModel);
                }
            }
        }
        #endregion
    }
}

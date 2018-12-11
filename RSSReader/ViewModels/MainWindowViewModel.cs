using System;
using System.Collections.Generic;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using Infrastructure.Constants;
using Infrastructure.Events;
using Infrastructure.Models;
using Infrastructure.Services;
using Infrastructure.ViewModels;
using ModuleAdd.Views;
using ModuleArchiveFeeds.Views;
using ModuleFeeds.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Logging;
using Prism.Regions;

namespace RSSReader.ViewModels
{
	public class MainWindowViewModel : BindableBase
	{
	    private readonly IRegionManager _regionManager;
	    private ICollection<Source> _sourceList;
	    private readonly IEventAggregator _eventAggregator;
	    private readonly ISourceStore _sourceStore;
        private readonly ILoggerFacade _logger = ProjectLogger.GetLogger;
        private Source _currentSource;

	    public MainWindowViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, ISourceStore sourceStore)
	    {
            var newSource = new Source
            {
                FeedUri = new Uri("https://www.heise.de/newsticker/heise-atom.xml"),
                Name = "Heise online",
                Category = "Technik"
            };

            var newSource1 = new Source
            {
                FeedUri = new Uri("https://www.heise.de/developer/rss/news-atom.xml"),
                Name = "Heise Developer",
                Category = "Technik"
            };

	        _regionManager = regionManager;
	        _eventAggregator = eventAggregator;
	        _sourceStore = sourceStore;

            _sourceList = new ObservableCollection<Source> {newSource, newSource1};
            _sourceStore.SafeAllSources(AllSources);
            //LoadSources();

	        GetSourceDelegateCommand = new DelegateCommand<Source>(SetCurrentSource);
	        GetAllSourcesDelegateCommand = new DelegateCommand(GetAllSources);

            UpdateFeedsDelegateCommand = new DelegateCommand(UpdateFeeds);

            OpenAddFeedWindowDelegateCommand = new DelegateCommand(OpenAddFeedWindow);
            OpenEditFeedWindowDelegateCommand = new DelegateCommand(OpenEditFeedWindow);

	        ShowArchiveFeedsDelegateCommand = new DelegateCommand(ShowArchiveFeeds);
	        ShowFeedsDelegateCommand = new DelegateCommand(ShowFeeds);


	        eventAggregator.GetEvent<NewSourceEvent>().Subscribe(AddSource);
	    }

        #region delegates
        public DelegateCommand<Source> GetSourceDelegateCommand { get; }
        public DelegateCommand UpdateFeedsDelegateCommand { get; }
	    public DelegateCommand OpenAddFeedWindowDelegateCommand { get; }
	    public DelegateCommand OpenEditFeedWindowDelegateCommand { get; }
	    public DelegateCommand GetAllSourcesDelegateCommand { get; }
        public DelegateCommand ShowFeedsDelegateCommand { get; }
        public DelegateCommand ShowArchiveFeedsDelegateCommand { get; }
        #endregion

        #region attributes
	    public ICollection<Source> AllSources
	    {
	        get => _sourceList;
	        private set => SetProperty(ref _sourceList, value);
	    }

	    public Source CurrentSource
        {
            get => _currentSource;
            set => SetProperty(ref _currentSource, value);
        }
        #endregion

        #region helper
        /// <summary>
        /// Sets the source that will be handled
        /// </summary>
        /// <param name="source"></param>
        private void SetCurrentSource(Source source)
        {
            ShowFeeds();
            _logger.Log("Set the Current source to: " + source.Name + ", " + source.FeedUri + ", " + source.Category, Category.Info, Priority.Medium);
	        CurrentSource = source;
            _eventAggregator.GetEvent<WantFeedEvent>().Publish(source);
	    }

        /// <summary>
        /// Each source will be used as one big source
        /// </summary>
        private void GetAllSources()
	    {
            ShowFeeds();
            _logger.Log("Get feeds from all sources", Category.Info, Priority.Medium);
	        CurrentSource = null;
            _eventAggregator.GetEvent<WantAllFeedsEvent>().Publish(_sourceList);
	    }

        /// <summary>
        /// Adds a new source to the list. Must be a rss-source like .xml or .atom
        /// </summary>
        /// <param name="source"></param>
	    private void AddSource(Source source)
	    {
            _logger.Log("Add the source to: " + source.Name + ", " + source.FeedUri + ", " + source.Category, Category.Info, Priority.Medium);
            _sourceList.Add(source);
            _sourceStore.DeleteAllSources();
            _sourceStore.SafeAllSources(AllSources);
	    }

        /// <summary>
        /// Fetch potential new feeds from the current source
        /// </summary>
	    private void UpdateFeeds()
	    {
            _logger.Log("Update feeds", Category.Info, Priority.Medium);
            _eventAggregator.GetEvent<FetchDataEvent>().Publish(true);
	    }

        //TODO: Auslagern
	    private void LoadSources()
	    {
	        AllSources = _sourceStore.GetAllSources();
	    }

	    private bool RemoveSource(Source source)
	    {
	        foreach (var oneSource in AllSources)
	        {
	            if (oneSource.Equals(source))
	            {
                    _sourceStore.DeleteAllSources();
                    _sourceStore.SafeAllSources(AllSources);
	                return AllSources.Remove(source);
	            }
	        }

	        return false;
	    }
        #endregion

        #region navigation management

	    private void ShowFeeds()
	    {
            _regionManager.RequestNavigate(RegionNames.ContentRegionLeft, nameof(FeedBoxUserControl));
	    }

	    private void ShowArchiveFeeds()
	    {
            _regionManager.RequestNavigate(RegionNames.ContentRegionLeft, nameof(ArchiveFeedBoxUserControl));
	    }

        private void OpenAddFeedWindow()
	    {
	        new SecondWindow().Show();
	    }

	    private void OpenEditFeedWindow()
	    {
	        new SecondWindow().Show();
	    }
        #endregion
    }
}

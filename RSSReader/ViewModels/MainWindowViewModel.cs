using System;
using System.Collections;
using System.Collections.Generic;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Linq;
using Infrastructure.Constants;
using Infrastructure.Events;
using Infrastructure.Models;
using Infrastructure.Services;
using ModuleAdd.ViewModels;
using ModuleAdd.Views;
using ModuleArchiveFeeds.Views;
using ModuleEdit.Views;
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
	    private bool _showBrowserFlag;
	    private readonly IRssStore _sourceStore;
        private readonly ILoggerFacade _logger = ProjectLogger.GetLogger;
        private Source _currentSource;

	    public MainWindowViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, IRssStore sourceStore)
	    {
            /*
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

            _sourceList = new ObservableCollection<Source> {newSource, newSource1};
            _sourceStore.SafeAllSources(AllSources);
            */

	        _regionManager = regionManager;
	        _eventAggregator = eventAggregator;
	        _sourceStore = sourceStore;

            LoadSources();

	        GetSourceDelegateCommand = new DelegateCommand<Source>(SetCurrentSource);
	        GetAllSourcesDelegateCommand = new DelegateCommand(GetAllSources);

            UpdateFeedsDelegateCommand = new DelegateCommand(UpdateFeeds);

            OpenAddFeedWindowDelegateCommand = new DelegateCommand(OpenAddFeedWindow);
            OpenEditFeedWindowDelegateCommand = new DelegateCommand(OpenEditFeedWindow);

	        ShowArchiveFeedsDelegateCommand = new DelegateCommand(ShowArchiveFeeds);
	        ShowFeedsDelegateCommand = new DelegateCommand(ShowFeeds);

            eventAggregator.GetEvent<WantUriEvent>().Subscribe(OpenBrowser);
            eventAggregator.GetEvent<WantCloseUriEvent>().Subscribe(CloseBrowser);


            eventAggregator.GetEvent<EditSourceEvent>().Subscribe(EditSource);
            eventAggregator.GetEvent<NewSourceEvent>().Subscribe(AddSource);
            eventAggregator.GetEvent<RemoveSourceEvent>().Subscribe(RemoveSource);
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

	    public bool BrowserFlag
	    {
	        get => _showBrowserFlag;
	        set => SetProperty(ref _showBrowserFlag, value);
	    }

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
        private void OpenBrowser(Uri uri)
        {
            BrowserFlag = true;
        }

        private void CloseBrowser()
        {
            BrowserFlag = false;
        }

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

	    private void EditSource(Source source)
	    {
	        _logger.Log("Safe the edit source " + source.Name + ", ID: " + source.Id, Category.Info, Priority.Medium);
            RemoveSource(source);
            AddSource(source);
	    }

	    private void RemoveSource(Source source)
	    {
	        foreach (var oneSource in AllSources)
	        {
	            if (source.Id == oneSource.Id)
	            {
	                AllSources.Remove(oneSource);
	                break;
	            }
	        }
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

        #endregion

        #region navigation management

	    private void ShowFeeds()
	    {
	        BrowserFlag = false;
            _regionManager.RequestNavigate(RegionNames.ContentRegionLeft, nameof(FeedBoxUserControl));
	    }

	    private void ShowArchiveFeeds()
	    {
	        BrowserFlag = false;
            _regionManager.RequestNavigate(RegionNames.ContentRegionLeft, nameof(ArchiveFeedBoxUserControl));
	    }

        private void OpenAddFeedWindow()
	    {
	        new AddFeedWindow().Show();
	    }

	    private void OpenEditFeedWindow()
	    {
	        new EditFeedFormWindow().Show();
	    }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using Prism.Mvvm;
using Infrastructure.Constants;
using Infrastructure.Events;
using Infrastructure.Models;
using Infrastructure.Services;
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
    /// <summary>
    /// Viewmodel for the main window
    /// </summary>
	public class MainWindowViewModel : BindableBase
	{
	    private readonly IRegionManager _regionManager;
	    private ICollection<Source> _sourceList;
	    private readonly IEventAggregator _eventAggregator;
	    private bool _showBrowserFlag;
        private bool _isUpdating;
        private readonly IRssStore _sourceStore;
        private readonly ILoggerFacade _logger = ProjectLogger.GetLogger;
        private Source _currentSource;

	    public MainWindowViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, IRssStore sourceStore)
	    {
	        _regionManager = regionManager;
	        _eventAggregator = eventAggregator;
	        _sourceStore = sourceStore;

            LoadSources();

	        GetSourceDelegateCommand = new DelegateCommand<Source>(SetCurrentSource);
	        GetAllSourcesDelegateCommand = new DelegateCommand(GetAllSources);

            UpdateFeedsDelegateCommand = new DelegateCommand(UpdateFeeds);

            OpenAddSourceWindowDelegateCommand = new DelegateCommand(OpenAddFeedWindow);
            OpenEditFeedWindowDelegateCommand = new DelegateCommand(OpenEditFeedWindow);

	        ShowArchiveFeedsDelegateCommand = new DelegateCommand(ShowArchiveFeeds);
	        ShowFeedsDelegateCommand = new DelegateCommand(ShowFeeds);

            eventAggregator.GetEvent<WantUriEvent>().Subscribe(OpenBrowser);
            eventAggregator.GetEvent<WantCloseUriEvent>().Subscribe(CloseBrowser);


            eventAggregator.GetEvent<EditSourceEvent>().Subscribe(EditSource);
            eventAggregator.GetEvent<NewSourceEvent>().Subscribe(AddSource);
            eventAggregator.GetEvent<RemoveSourceEvent>().Subscribe(RemoveSource);
            eventAggregator.GetEvent<FeedsLoadedEvent>().Subscribe(FeedsLoaded);
	    }

        #region delegates
        /// <summary>
        /// Handles the interaction that the user wants to see the feeds for a source
        /// </summary>
        public DelegateCommand<Source> GetSourceDelegateCommand { get; }

        /// <summary>
        /// Handles the interaction that the user wants to update/fetch the feed list
        /// </summary>
        public DelegateCommand UpdateFeedsDelegateCommand { get; }

        /// <summary>
        /// Handles the interaction that the user wants to add a new source
        /// </summary>
	    public DelegateCommand OpenAddSourceWindowDelegateCommand { get; }

        /// <summary>
        /// Handles the interaction that the user wants to edit the sources
        /// </summary>
	    public DelegateCommand OpenEditFeedWindowDelegateCommand { get; }

        /// <summary>
        /// Handles the interaction that the user wants to see the feeds from all sources
        /// </summary>
	    public DelegateCommand GetAllSourcesDelegateCommand { get; }

        /// <summary>
        /// Handles the interaction that the user wants to see feeds
        /// </summary>
        public DelegateCommand ShowFeedsDelegateCommand { get; }

        /// <summary>
        /// Handles the interaction that the user wants to see the archived feeds
        /// </summary>
        public DelegateCommand ShowArchiveFeedsDelegateCommand { get; }
        #endregion

        #region attributes
        /// <summary>
        /// Gets and Sets the Updating process
        /// </summary>
        public bool IsUpdating
        {
            get => _isUpdating;
            set => SetProperty(ref _isUpdating, value);
        }

        /// <summary>
        /// Gets and Sets the BrowserFlag. This is necessary to split the window
        /// </summary>
        public bool BrowserFlag
	    {
	        get => _showBrowserFlag;
	        set => SetProperty(ref _showBrowserFlag, value);
	    }

        /// <summary>
        /// Gets and Sets all sources that are available
        /// </summary>
	    public ICollection<Source> AllSources
	    {
	        get => _sourceList;
	        private set => SetProperty(ref _sourceList, value);
	    }

        /// <summary>
        /// Gets and Sets the current shown source
        /// </summary>
	    public Source CurrentSource
        {
            get => _currentSource;
            set => SetProperty(ref _currentSource, value);
        }
        #endregion

        #region helper
        /// <summary>
        /// Sets the Variable isUpdating on false after the feeds are loaded successfully
        /// </summary>
        /// <param name="flag"></param>
        private void FeedsLoaded (bool flag)
        {
            IsUpdating = false;
        }
        
        /// <summary>
        /// Opens the browser
        /// </summary>
        /// <param name="uri"></param>
        private void OpenBrowser(Uri uri)
        {
            BrowserFlag = true;
        }

        /// <summary>
        /// Closes the browser
        /// </summary>
        private void CloseBrowser()
        {
            BrowserFlag = false;
        }

        /// <summary>
        /// Sets the source that will be handled and publishes an event that the user wants to see the feeds for the source
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
        /// Adds a new source to the list
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
        /// Saves the source after editing
        /// </summary>
        /// <param name="source">The Source that should be saved after editing</param>
	    private void EditSource(Source source)
	    {
	        _logger.Log("Safe the edit source " + source.Name + ", ID: " + source.Id, Category.Info, Priority.Medium);
            RemoveSource(source);
            AddSource(source);
	    }

        /// <summary>
        /// Removes a source from the list
        /// </summary>
        /// <param name="source">The source which should be removed from the list</param>
	    private void RemoveSource(Source source)
	    {
	        foreach (var oneSource in AllSources)
	        {
	            if (source.Equals(oneSource))
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
            IsUpdating = true;
            _logger.Log("Update feeds", Category.Info, Priority.Medium);
            _eventAggregator.GetEvent<FetchDataEvent>().Publish(true);
	    }

        /// <summary>
        /// Loads all sources saved from the user
        /// </summary>
	    private void LoadSources()
	    {
	        AllSources = _sourceStore.GetAllSources();
	    }

        #endregion

        #region navigation management

        /// <summary>
        /// Closes the browser and shows the feeds for a source
        /// </summary>
	    private void ShowFeeds()
	    {
	        BrowserFlag = false;
            _regionManager.RequestNavigate(RegionNames.ContentRegionLeft, nameof(FeedBoxUserControl));
	    }

        /// <summary>
        /// Closes the browser and shows the archived feeds from a source
        /// </summary>
	    private void ShowArchiveFeeds()
	    {
	        BrowserFlag = false;
            _regionManager.RequestNavigate(RegionNames.ContentRegionLeft, nameof(ArchiveFeedBoxUserControl));
	    }

        /// <summary>
        /// Opens a new Window to add new sources
        /// </summary>
        private void OpenAddFeedWindow()
	    {
	        new AddFeedWindow().Show();
	    }

        /// <summary>
        /// Opens a new Window to edit the sources
        /// </summary>
	    private void OpenEditFeedWindow()
	    {
	        new EditFeedFormWindow().Show();
	    }
        #endregion
    }
}

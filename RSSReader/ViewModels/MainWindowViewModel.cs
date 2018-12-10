using System;
using System.Collections.Generic;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using Infrastructure.Constants;
using Infrastructure.Events;
using Infrastructure.Models;
using Infrastructure.Services;
using ModuleAdd.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Logging;
using Prism.Regions;

namespace RSSReader.ViewModels
{
	public class MainWindowViewModel : BindableBase
	{
	    private readonly IRegionManager _regionManager;
	    private readonly ICollection<Source> _sourceList;
	    private readonly IEventAggregator _eventAggregator;
        private readonly ILoggerFacade _logger = ProjectLogger.GetLogger;
        private Source _currentSource;

	    public MainWindowViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
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

	        _sourceList = new ObservableCollection<Source> {newSource, newSource1};

	        GetSourceDelegateCommand = new DelegateCommand<Source>(SetCurrentSource);
	        GetAllSourcesDelegateCommand = new DelegateCommand(GetAllSources);

            UpdateFeedsDelegateCommand = new DelegateCommand(UpdateFeeds);

            OpenAddFeedWindowDelegateCommand = new DelegateCommand(OpenAddFeedWindow);
            OpenEditFeedWindowDelegateCommand = new DelegateCommand(OpenEditFeedWindow);

	        eventAggregator.GetEvent<NewSourceEvent>().Subscribe(AddSource);
	    }

        #region delegates
        public DelegateCommand<Source> GetSourceDelegateCommand { get; }
        public DelegateCommand UpdateFeedsDelegateCommand { get; }
	    public DelegateCommand OpenAddFeedWindowDelegateCommand { get; }
	    public DelegateCommand OpenEditFeedWindowDelegateCommand { get; }
	    public DelegateCommand GetAllSourcesDelegateCommand { get; }
        #endregion

        #region attributes
        public ICollection<Source> AllSources => _sourceList;

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
            _logger.Log("Set the Current source to: " + source.Name + ", " + source.FeedUri + ", " + source.Category, Category.Info, Priority.Medium);
	        CurrentSource = source;
            _eventAggregator.GetEvent<WantFeedEvent>().Publish(source);
	    }

        /// <summary>
        /// Each source will be used as one big source
        /// </summary>
        private void GetAllSources()
	    {
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
	    }

        /// <summary>
        /// Fetch potential new feeds from the current source
        /// </summary>
	    private void UpdateFeeds()
	    {
            _logger.Log("Update feeds", Category.Info, Priority.Medium);
            _eventAggregator.GetEvent<FetchDataEvent>().Publish(true);
	    }
        #endregion

        #region navigation management
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

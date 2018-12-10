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

	        _regionManager = regionManager;
	        _eventAggregator = eventAggregator;

	        _sourceList = new ObservableCollection<Source> {newSource};

	        SetSourceDelegateCommand = new DelegateCommand<Source>(SetCurrentSource);
            UpdateFeedsDelegateCommand = new DelegateCommand(UpdateFeeds);
            OpenAddFeedWindowDelegateCommand = new DelegateCommand(OpenAddFeedWindow);
            OpenEditFeedWindowDelegateCommand = new DelegateCommand(OpenEditFeedWindow);

	        eventAggregator.GetEvent<NewSourceEvent>().Subscribe(AddSource);
	    }

        #region delegates
        public DelegateCommand<Source> SetSourceDelegateCommand { get; }
        public DelegateCommand UpdateFeedsDelegateCommand { get; }
	    public DelegateCommand OpenAddFeedWindowDelegateCommand { get; }
	    public DelegateCommand OpenEditFeedWindowDelegateCommand { get; }
        #endregion

        public ICollection<Source> AllSources => _sourceList;

	    public Source CurrentSource
        {
            get => _currentSource;
            set => SetProperty(ref _currentSource, value);
        }

        #region helper
        private void SetCurrentSource(Source source)
	    {
	        CurrentSource = source;
	    }

	    private void AddSource(Source newSource)
	    {
            _sourceList.Add(newSource);
	    }

	    private void UpdateFeeds()
	    {
            _eventAggregator.GetEvent<FetchDataEvent>().Publish(true);
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

using System;
using System.Collections.Generic;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using Infrastructure.Models;
using Infrastructure.Services;
using Prism.Commands;
using Prism.Regions;

namespace RSSReader.ViewModels
{
	public class MainWindowViewModel : BindableBase
	{
	    private readonly IRegionManager _regionManager;
	    private readonly ICollection<Source> _sourceList; 
        private Source _currentSource;

	    public MainWindowViewModel(IRegionManager regionManager)
	    {
            var newSource = new Source
            {
                FeedUri = new Uri("https://www.heise.de/newsticker/heise-atom.xml"),
                Name = "Heise online",
                Category = "Technik"
            };

	        _regionManager = regionManager;
	        _sourceList = new ObservableCollection<Source>();
            _sourceList.Add(newSource);
	        SetSourceDelegateCommand = new DelegateCommand<Source>(SetCurrentSource);
	    }

        public DelegateCommand<Source> SetSourceDelegateCommand { get; }

	    public ICollection<Source> AllSources => _sourceList;

	    private void SetCurrentSource(Source source)
	    {
	        CurrentSource = source;
	     //   _feedBoxUserControlViewModel.CurrentUri = source.FeedUri;
	    }

	    public Source CurrentSource
        {
            get => _currentSource;
            set => SetProperty(ref _currentSource, value);
        }
    }
}

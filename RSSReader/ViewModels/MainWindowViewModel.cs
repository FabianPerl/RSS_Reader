using System;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using Prism.Commands;
using RSSReader.Models;

namespace RSSReader.ViewModels
{
	public class MainWindowViewModel : BindableBase
	{
	    private readonly ObservableCollection<Source> _sourceList = SingletonList<Source>.GetInstance;
	    private readonly FeedBoxUserControlViewModel _feedBoxUserControlViewModel;
        private Source _currentSource;

	    public MainWindowViewModel()
	    {
            var newSource = new Source
            {
                FeedUri = new Uri("https://www.heise.de/newsticker/heise-atom.xml"),
                Name = "Heise online",
                Category = "Technik"
            };

            _feedBoxUserControlViewModel = new FeedBoxUserControlViewModel();
            _sourceList.Add(newSource);

            SetSourceDelegateCommand = new DelegateCommand<Source>(SetCurrentSource);
	    }

        public DelegateCommand<Source> SetSourceDelegateCommand { get; }

	    public Collection<Source> AllSources => _sourceList;

	    private void SetCurrentSource(Source source)
	    {
	        CurrentSource = source;
	        _feedBoxUserControlViewModel.CurrentUri = source.FeedUri;
	    }

	    public Source CurrentSource
        {
            get => _currentSource;
            set
            {
                SetProperty(ref _currentSource, value);
                //UpdateFeedList(value.FeedUri);
            }
        }
    }
}

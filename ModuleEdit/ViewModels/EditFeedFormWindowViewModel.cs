using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Constants;
using Infrastructure.Events;
using Infrastructure.Models;
using Infrastructure.Services;
using Prism.Events;
using Prism.Logging;

namespace ModuleEdit.ViewModels
{
    public class EditFeedFormWindowViewModel : BindableBase
    {
        private readonly ILoggerFacade _logger = ProjectLogger.GetLogger;
        private readonly IEventAggregator _eventAggregator;
        private readonly IRssStore _rssStore;
        private Source _sourceToEdit;

        public EditFeedFormWindowViewModel(IEventAggregator eventAggregator, IRssStore rssStore)
        {
            _eventAggregator = eventAggregator;
            _rssStore = rssStore;
            AllSources = _rssStore.GetAllSources();
            RemoveSourceCommand = new DelegateCommand<Source>(RemoveOneSource);
            EditSourceCommand = new DelegateCommand(EditOneSource);
            PreviewEditSourceCommand = new DelegateCommand<Source>(PreviewOneSource);

            if(AllSources.Count >= 1)
                SourceToEdit = AllSources.ElementAt(0);
        }

        public ICollection<Source> AllSources { get; }
        public DelegateCommand<Source> RemoveSourceCommand { get; }
        public DelegateCommand EditSourceCommand { get; }
        public DelegateCommand<Source> PreviewEditSourceCommand { get; }

        private string _name;
        public string NameOfSource
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private Uri _uri;
        public Uri UriOfSource
        {
            get => _uri;
            set => SetProperty(ref _uri, value);
        }

        private string _category;
        public string CategoryOfSource
        {
            get => _category;
            set => SetProperty(ref _category, value);
        }

        public Source SourceToEdit
        {
            get => _sourceToEdit;
            set
            {
                SetProperty(ref _sourceToEdit, value);
                CategoryOfSource = value.Category;
                UriOfSource = value.FeedUri;
                NameOfSource = value.Name;
            }
        }

        #region helper
        private void EditOneSource()
        {

        }
        
        private void PreviewOneSource(Source source)
        {
            _logger.Log("Preview " + source.Name + " with URI " + source.FeedUri, Category.Info, Priority.Medium);   
            SourceToEdit = source;
        }

        private void RemoveOneSource(Source source)
        {
            _eventAggregator.GetEvent<RemoveSourceEvent>().Publish(source);
        }
        #endregion
    }
}

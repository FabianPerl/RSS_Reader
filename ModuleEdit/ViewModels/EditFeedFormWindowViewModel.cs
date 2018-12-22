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
            RemoveSourceCommand = new DelegateCommand(RemoveOneSource);
            EditSourceCommand = new DelegateCommand(EditOneSource);
            PreviewEditSourceCommand = new DelegateCommand<Source>(PreviewOneSource);

            if(AllSources.Count >= 1)
                SourceToEdit = AllSources.ElementAt(0);


            foreach (var value in AllSources)
            {
                _logger.Log("INITIALIZE: " + value.Name + " with URI " + value.FeedUri + " and ID: " + value.Id, Category.Info, Priority.Medium);   
            }

        }

        public ICollection<Source> AllSources { get; }
        public DelegateCommand RemoveSourceCommand { get; }
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
                _logger.Log("Edit: " + value.Name + " with URI " + value.FeedUri + " and ID: " + value.Id, Category.Info, Priority.Medium);   
                SetProperty(ref _sourceToEdit, value);
                CategoryOfSource = value.Category;
                UriOfSource = value.FeedUri;
                NameOfSource = value.Name;
            }
        }

        #region helper
        private void EditOneSource()
        {
            _sourceToEdit.FeedUri = UriOfSource;
            _sourceToEdit.Name = NameOfSource;
            _sourceToEdit.Category = CategoryOfSource;
            _eventAggregator.GetEvent<EditSourceEvent>().Publish(_sourceToEdit);
        }
        
        private void PreviewOneSource(Source source)
        {
            _logger.Log("Preview " + source.Name + " with URI " + source.FeedUri + " and ID: " + source.Id, Category.Info, Priority.Medium);   
            SourceToEdit = source;
        }

        private void RemoveOneSource()
        {
            _logger.Log("Remove " + _sourceToEdit.Name + " with URI " + _sourceToEdit.FeedUri + " and ID: " + _sourceToEdit.Id, Category.Info, Priority.Medium);   
            _eventAggregator.GetEvent<RemoveSourceEvent>().Publish(_sourceToEdit);
        }
        #endregion
    }
}

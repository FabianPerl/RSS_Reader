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

        public string NameOfSource
        {
            get => _sourceToEdit?.Name;
            set
            {
                if (_sourceToEdit == null) return;
                _sourceToEdit.Name = value;
            }
        }

        public Uri UriOfSource
        {
            get => _sourceToEdit?.FeedUri;
            set
            {
                if (_sourceToEdit == null) return;
                _sourceToEdit.FeedUri = value;
            }
        }

        public string CategoryOfSource
        {
            get => _sourceToEdit?.Category;
            set
            {
                if (_sourceToEdit == null) return;
                _sourceToEdit.Category = value;
            }
        }

        public Source SourceToEdit
        {
            get => _sourceToEdit;
            set => SetProperty(ref _sourceToEdit, value);
        }

        #region helper
        private void EditOneSource()
        {

        }
        private void PreviewOneSource(Source source)
        {
            SourceToEdit = source;
        }

        private void RemoveOneSource(Source source)
        {
            _eventAggregator.GetEvent<RemoveSourceEvent>().Publish(source);
        }
        #endregion
    }
}

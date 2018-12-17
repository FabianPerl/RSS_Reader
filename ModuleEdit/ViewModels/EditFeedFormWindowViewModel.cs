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
        private Source _soureToEdit;

        public EditFeedFormWindowViewModel(IEventAggregator eventAggregator, IRssStore rssStore)
        {
            _eventAggregator = eventAggregator;
            _rssStore = rssStore;
            AllSources = _rssStore.GetAllSources();
            RemoveSourceCommand = new DelegateCommand<Source>(RemoveOneSource);
            EditSourceCommand = new DelegateCommand<Source>(EditOneSource);
        }

        public ICollection<Source> AllSources { get; }
        public DelegateCommand<Source> RemoveSourceCommand;
        public DelegateCommand<Source> EditSourceCommand;

        public Source SourceToEdit
        {
            get => _soureToEdit;
            set => SetProperty(ref _soureToEdit, value);
        }

        #region helper
        private void EditOneSource(Source source)
        {

        }

        private void RemoveOneSource(Source source)
        {
            _eventAggregator.GetEvent<RemoveSourceEvent>().Publish(source);
        }
        #endregion
    }
}

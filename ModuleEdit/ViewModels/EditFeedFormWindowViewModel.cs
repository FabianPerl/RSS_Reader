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

        public EditFeedFormWindowViewModel(IEventAggregator eventAggregator, IRssStore rssStore)
        {
            _eventAggregator = eventAggregator;
            _rssStore = rssStore;
            AllSources = _rssStore.GetAllSources();
            RemoveSource = new DelegateCommand<Source>(RemoveOneSource);
        }


        public ICollection<Source> AllSources { get; }

        public DelegateCommand<Source> RemoveSource;

        private void RemoveOneSource(Source source)
        {
            _eventAggregator.GetEvent<RemoveSourceEvent>().Publish(source);
        }
    }
}

using System;
using Infrastructure.Constants;
using Infrastructure.Events;
using Infrastructure.Models;
using Prism.Commands;
using Prism.Events;
using Prism.Logging;
using Prism.Mvvm;

namespace ModuleAdd.ViewModels
{
    public class AddFeedFormUserControlViewModel : BindableBase
    {
        private string _name;
        private Uri _uri;
        private string _category;
        private readonly ILoggerFacade _logger = ProjectLogger.GetLogger;
        private readonly IEventAggregator _eventAggregator;

        public AddFeedFormUserControlViewModel(IEventAggregator eventAggregator)
        {
            AddCommand = new DelegateCommand(Execute, CanExecute).ObservesProperty(() => Name).
                ObservesProperty(() => Category).
                ObservesProperty(() => Uri);

            _eventAggregator = eventAggregator;
        }

        #region delegates
        public DelegateCommand AddCommand { get; set; }
        #endregion

        #region attributes
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string Category
        {
            get => _category;
            set => SetProperty(ref _category, value);
        }

        public Uri Uri
        {
            get => _uri;
            set => SetProperty(ref _uri, value);
        }
        #endregion

        #region helper
        private bool CanExecute()
        {
            return !string.IsNullOrWhiteSpace(Name) &&
                   !string.IsNullOrWhiteSpace(Category) &&
                   !string.IsNullOrWhiteSpace(Uri.ToString());
        }

        private void Execute()
        {
            var newSource = new Source {Name = Name, Category = Category, FeedUri = Uri};
            _logger.Log("Add new Source: " + 
                             newSource.Name + ", " + 
                             newSource.Category + ", " + 
                             newSource.FeedUri, Prism.Logging.Category.Info, Priority.Medium);

            _eventAggregator.GetEvent<NewSourceEvent>().Publish(newSource);
        }
        #endregion
    }
}
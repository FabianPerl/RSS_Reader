using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Constants;
using Infrastructure.Events;
using Infrastructure.Models;
using Prism.Commands;
using Prism.Events;
using Prism.Logging;
using Prism.Mvvm;

namespace ModuleAdd.ViewModels
{
    /// <summary>
    /// Viewmodel for adding a new Source
    /// </summary>
    public class AddSourceWindowViewModel : BindableBase
    {
        private string _name;
        private Uri _uri;
        private string _category;
        private readonly ILoggerFacade _logger = ProjectLogger.GetLogger;
        private readonly IEventAggregator _eventAggregator;

        public AddSourceWindowViewModel(IEventAggregator eventAggregator)
        {
            _logger.Log("Initialize the viewmodel for add source", Prism.Logging.Category.Info, Priority.Medium);
            AddCommand = new DelegateCommand(Execute, CanExecute).ObservesProperty(() => Name).
                ObservesProperty(() => Category).
                ObservesProperty(() => Uri);

            _eventAggregator = eventAggregator;
        }

        #region delegates
        /// <summary>
        /// Command to check if the source is addable
        /// </summary>
        public DelegateCommand AddCommand { get; }
        #endregion

        #region attributes
        /// <summary>
        /// Gets all available categories for the Source
        /// </summary>
        public IEnumerable<Categories> Categories { get; } = Enum.GetValues(typeof(Categories)).Cast<Categories>();

        /// <summary>
        /// Gets and Sets the Name for te Source
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        /// <summary>
        /// Gets and Sets the Category for the Source
        /// </summary>
        public string Category
        {
            get => _category;
            set => SetProperty(ref _category, value);
        }

        /// <summary>
        /// Gets and Sets the Uri for the Source
        /// </summary>
        public Uri Uri
        {
            get => _uri;
            set => SetProperty(ref _uri, value);
        }
        #endregion

        #region helper

        /// <summary>
        /// Checks if the Source can be added or not
        /// </summary>
        /// <returns>True if and only if all fields are not null or empty or has errors</returns>
        private bool CanExecute()
        {
            return !string.IsNullOrWhiteSpace(Name) &&
                   !string.IsNullOrWhiteSpace(Category) &&
                   !string.IsNullOrWhiteSpace(Uri.ToString());
        }

        /// <summary>
        /// Creates the new Source and publishes it as an event
        /// </summary>
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
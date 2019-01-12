using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Constants;
using Infrastructure.Events;
using Infrastructure.Models;
using Infrastructure.Services;
using Prism.Events;
using Prism.Logging;

namespace ModuleEdit.ViewModels
{
    /// <summary>
    /// Viewmodel for the window edit feed
    /// </summary>
    public class EditFeedFormWindowViewModel : BindableBase
    {
        private readonly ILoggerFacade _logger = ProjectLogger.GetLogger;
        private readonly IEventAggregator _eventAggregator;
        private Source _sourceToEdit;

        public EditFeedFormWindowViewModel(IEventAggregator eventAggregator, IRssStore rssStore)
        {
            _eventAggregator = eventAggregator;
            AllSources = rssStore.GetAllSources();

            PreviewEditSourceCommand = new DelegateCommand<Source>(PreviewOneSource);
            RemoveSourceCommand = new DelegateCommand(RemoveOneSource);
            EditSourceCommand = new DelegateCommand(EditOneSource, CanExecute).
                ObservesProperty(() => NameOfSource).
                ObservesProperty(() => CategoryOfSource).
                ObservesProperty(() => UriOfSource);

            if(AllSources.Count >= 1)
                SourceToEdit = AllSources.ElementAt(0);
        }

        #region delegates
        /// <summary>
        /// Handles the interaction that the user wants to remove the clicked source
        /// </summary>
        public DelegateCommand RemoveSourceCommand { get; }

        /// <summary>
        /// Handles the interaction that the user wants to edit the clicked source
        /// </summary>
        public DelegateCommand EditSourceCommand { get; }

        /// <summary>
        /// Handles the interaction that the user wants to see the saved data from the clicked source
        /// </summary>
        public DelegateCommand<Source> PreviewEditSourceCommand { get; }
        #endregion

        #region attributes
        /// <summary>
        /// Gets all available categories for the Source
        /// </summary>
        public IEnumerable<Categories> Categories { get; } = Enum.GetValues(typeof(Categories)).Cast<Categories>();

        /// <summary>
        /// Gets all Sources saved from the user
        /// </summary>
        public ICollection<Source> AllSources { get; }

        private string _name;

        /// <summary>
        /// Gets and Sets the name of the clicked Source
        /// </summary>
        public string NameOfSource
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private Uri _uri;

        /// <summary>
        /// Gets and Sets the uri of the clicked Source
        /// </summary>
        public Uri UriOfSource
        {
            get => _uri;
            set => SetProperty(ref _uri, value);
        }

        private bool _hasErrors;

        /// <summary>
        /// Gets and Sets the errors 
        /// </summary>
        private bool HasErrors
        {
            get => _hasErrors;
            set => SetProperty(ref _hasErrors, value);
        }

        private string _category;

        /// <summary>
        /// Gets and Sets the category of the clicked Source
        /// </summary>
        public string CategoryOfSource
        {
            get => _category;
            set => SetProperty(ref _category, value);
        }

        private bool _hasValues;

        /// <summary>
        /// Gets and Sets the state of the edit source
        /// </summary>
        public bool HasValues
        {
            get => _hasValues;
            set => SetProperty(ref _hasValues, value);
        }

        /// <summary>
        /// Gets and Sets the Source which should be shown in the preview
        /// </summary>
        public Source SourceToEdit
        {
            get => _sourceToEdit;
            set
            {
                _logger.Log("Edit: " + value.Name + " with URI " + value.FeedUri + " and ID: " + value.Id, Category.Info, Priority.Medium);
                HasValues = true;
                SetProperty(ref _sourceToEdit, value);
                SourceToEdit.Id = value.Id;
                CategoryOfSource = value.Category;
                UriOfSource = value.FeedUri;
                NameOfSource = value.Name;
            }
        }
        #endregion

        #region helper
        /// <summary>
        /// Initializes variables with the edited source and publishes an event that the source was edited
        /// </summary>
        private void EditOneSource()
        {
            if (_sourceToEdit == null) return;

            _sourceToEdit.FeedUri = UriOfSource;
            _sourceToEdit.Name = NameOfSource;
            _sourceToEdit.Category = CategoryOfSource;
            _eventAggregator.GetEvent<EditSourceEvent>().Publish(_sourceToEdit);
        }
        
        /// <summary>
        /// Shows the data saved from the source
        /// </summary>
        /// <param name="source">The source that should be shown</param>
        private void PreviewOneSource(Source source)
        {
            _logger.Log("Preview " + source.Name + " with URI " + source.FeedUri + " and ID: " + source.Id, Category.Info, Priority.Medium);   
            SourceToEdit = source;
        }

        /// <summary>
        ///  Publishes an event with the source that should be removed
        /// </summary>
        private void RemoveOneSource()
        {
            if (_sourceToEdit == null) return;

            _logger.Log( "Remove " + _sourceToEdit.Name + " with URI " + _sourceToEdit.FeedUri + " and ID: " + _sourceToEdit.Id, Category.Info, Priority.Medium);
            _eventAggregator.GetEvent<RemoveSourceEvent>().Publish(_sourceToEdit);
        }

        private bool CanExecute()
        {
            return !string.IsNullOrWhiteSpace(NameOfSource) &&
                   !string.IsNullOrWhiteSpace(CategoryOfSource) &&
                   !string.IsNullOrWhiteSpace(UriOfSource.ToString()) &&
                   !HasErrors;
        }
        #endregion
    }
}

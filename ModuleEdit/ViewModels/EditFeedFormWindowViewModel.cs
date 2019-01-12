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
            RemoveSourceCommand = new DelegateCommand(RemoveOneSource);
            EditSourceCommand = new DelegateCommand(EditOneSource);
            PreviewEditSourceCommand = new DelegateCommand<Source>(PreviewOneSource);

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

        /// <summary>
        /// Gets and Sets the name of the clicked Source
        /// </summary>
        private string _name;
        public string NameOfSource
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        /// <summary>
        /// Gets and Sets the uri of the clicked Source
        /// </summary>
        private Uri _uri;
        public Uri UriOfSource
        {
            get => _uri;
            set => SetProperty(ref _uri, value);
        }

        /// <summary>
        /// Gets and Sets the category of the clicked Source
        /// </summary>
        private string _category;
        public string CategoryOfSource
        {
            get => _category;
            set => SetProperty(ref _category, value);
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
                SetProperty(ref _sourceToEdit, value);
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
            _logger.Log("Remove " + _sourceToEdit.Name + " with URI " + _sourceToEdit.FeedUri + " and ID: " + _sourceToEdit.Id, Category.Info, Priority.Medium);   
            _eventAggregator.GetEvent<RemoveSourceEvent>().Publish(_sourceToEdit);
        }
        #endregion
    }
}

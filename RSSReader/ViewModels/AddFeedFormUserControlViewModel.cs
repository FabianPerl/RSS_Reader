using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Prism.Logging;
using RSSReader.Models;

namespace RSSReader.ViewModels
{
    public class AddFeedFormUserControlViewModel : BindableBase
    {
        private string _name;
        private Uri _uri;
        private string _category;
        private readonly DebugLogger _debugLogger = new DebugLogger();
        private readonly SourceList _sourceList = SourceList.GetInstance;

        public DelegateCommand AddCommand { get; set; }

        public AddFeedFormUserControlViewModel()
        {
            AddCommand = new DelegateCommand(Execute, CanExecute).ObservesProperty(() => Name).
                ObservesProperty(() => Category).
                ObservesProperty(() => Uri);
        }

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

        private bool CanExecute()
        {
            return !string.IsNullOrWhiteSpace(Name) &&
                   !string.IsNullOrWhiteSpace(Category) &&
                   !string.IsNullOrWhiteSpace(Uri.ToString());
        }

        private void Execute()
        {
            Source newSource = new Source();
            newSource.Name = Name;
            newSource.Category = Category;
            newSource.FeedUri = Uri;
            _debugLogger.Log("Add new Source: " + 
                             newSource.Name + ", " + 
                             newSource.Category + ", " + 
                             newSource.FeedUri, Prism.Logging.Category.Info, Priority.Medium);

            _sourceList.Add(newSource);
        }
    }
}
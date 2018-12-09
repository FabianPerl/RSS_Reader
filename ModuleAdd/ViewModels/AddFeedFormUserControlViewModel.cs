using System;
using System.Collections.Generic;
using Infrastructure.Models;
using Prism.Commands;
using Prism.Logging;
using Prism.Mvvm;

namespace ModuleAdd.ViewModels
{
    public class AddFeedFormUserControlViewModel : BindableBase
    {
        private string _name;
        private Uri _uri;
        private string _category;
        private readonly DebugLogger _debugLogger = new DebugLogger();
        //private readonly ObservableCollection<Source> _sourceList = SingletonList<Source>.GetInstance;
        private readonly ICollection<Source> _sourceList;

        public DelegateCommand AddCommand { get; set; }

        public AddFeedFormUserControlViewModel(ICollection<Source> sourceData)
        {
            AddCommand = new DelegateCommand(Execute, CanExecute).ObservesProperty(() => Name).
                ObservesProperty(() => Category).
                ObservesProperty(() => Uri);

            _sourceList = sourceData;
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
            var newSource = new Source {Name = Name, Category = Category, FeedUri = Uri};
            _debugLogger.Log("Add new Source: " + 
                             newSource.Name + ", " + 
                             newSource.Category + ", " + 
                             newSource.FeedUri, Prism.Logging.Category.Info, Priority.Medium);

            _sourceList.Add(newSource);
        }
    }
}
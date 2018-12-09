using System;
using Prism.Mvvm;

namespace Infrastructure.Models
{
    public class Source : BindableBase
    {
        private Uri _feedUri;
        private string _name;
        private string _category;

        public Uri FeedUri
        {
            get => _feedUri;
            set => SetProperty(ref _feedUri, value);
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
    }
}

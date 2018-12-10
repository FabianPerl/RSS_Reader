using System;
using Prism.Mvvm;

namespace Infrastructure.Models
{
    public class Source : BindableBase
    {
        private Uri _feedUri;
        private string _name;
        private string _category;

        public string Id { get; } = Guid.NewGuid().ToString();

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

        public override bool Equals(object source)
        {
            if (!(source is Source secondSource))
            {
                return false;
            }

            return this.Id.Equals(secondSource.Id);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}

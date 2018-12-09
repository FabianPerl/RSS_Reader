using System;
using Prism.Mvvm;

namespace Infrastructure.Models
{
    public class Source : BindableBase
    {
        private Uri _feedUri;
        // eventuell noch ein icon hinzufuegen
        private string _name;
        private string _category;

        public Uri FeedUri { get; set; }

        public string Name { get; set; }

        public string Category { get; set; }
    }
}

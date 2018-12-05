using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace RSSReader.Models
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

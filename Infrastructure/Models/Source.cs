using System;
using Newtonsoft.Json;
using Prism.Mvvm;

namespace Infrastructure.Models
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a source that spread out RSS feeds
    /// </summary>
    [JsonObject]
    public class Source : BindableBase
    {
        private Uri _feedUri;
        private string _name;
        private string _category;

        /// <summary>
        /// Gets the Source's individually ID
        /// </summary>
        [JsonProperty]
        public string Id { get; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets and Sets the Source's URL 
        /// </summary>
        [JsonProperty]
        public Uri FeedUri
        {
            get => _feedUri;
            set => SetProperty(ref _feedUri, value);
        }

        /// <summary>
        /// Gets and Sets the Source's Name
        /// </summary>
        [JsonProperty]
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        /// <summary>
        /// Gets and Sets the Source's Category. Used to categories the Source and to organize them
        /// </summary>
        [JsonProperty]
        public string Category
        {
            get => _category;
            set => SetProperty(ref _category, value);
        }

        /// <summary>
        /// Determines whether the specified Source is equal to the current Source 
        /// </summary>
        /// <param name="source">Source that should be compared to</param>
        /// <returns>True if and only if the Source has the same id, false otherwise</returns>
        public override bool Equals(object source)
        {
            if (!(source is Source secondSource))
            {
                return false;
            }

            return Id.Equals(secondSource.Id);
        }

        /// <summary>
        /// Gets the hashcode
        /// </summary>
        /// <returns>Returns the hash code of the Source, based on the id</returns>
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Prism.Mvvm;

namespace Infrastructure.ViewModels
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a feed from a source
    /// </summary>
    [JsonObject]
	public class FeedViewModel : BindableBase
	{
	    private string _imageUrl;
	    private ICollection<string> _authors;
	    private string _title;
	    private string _shortDescription;

	    private bool _isWatched;

	    private Uri _link;

	    private DateTimeOffset _publishedDate;
	    private DateTimeOffset _lastUpdatedTime;

        #region attributes

        /// <summary>
        /// Gets the Feed's individually ID
        /// </summary>
        [JsonProperty]
	    public string Id { get; } = Guid.NewGuid().ToString();

	    /// <summary>
        /// Get and Set the article's author(s).
        /// </summary>
        [JsonProperty]
        public ICollection<string> Authors
	    {
	        get => _authors;
	        set => SetProperty(ref _authors, value);
	    }

	    /// <summary>
	    /// Get and Set the article's Title.
	    /// </summary>
        [JsonProperty]
	    public string Title
	    {
	        get => _title;
	        set => SetProperty(ref _title, value);
	    }

	    /// <summary>
	    /// Get and Set the article's Description. It's a short summary for the article.
	    /// </summary>
        [JsonProperty]
	    public string ShortDescription
	    {
	        get => _shortDescription;
	        set
	        {
	            var filteredValue = Regex.Replace(value, "<.*?>", string.Empty);
	            SetProperty(ref _shortDescription, filteredValue);
	        }
	    }

	    /// <summary>
	    /// Get and Set the article's Link to access the feed itself.
	    /// </summary>
        [JsonProperty]
	    public Uri Link
	    {
	        get => _link;
	        set => SetProperty(ref _link, value);
	    }

        /// <summary>
        /// Gets and Sets the image/icon of the feed's source
        /// </summary>
        [JsonProperty]
	    public string ImageUrl
	    {
	        get => _imageUrl;
	        set => SetProperty(ref _imageUrl, value);
	    }

	    /// <summary>
	    /// Get and Set the article's Date when the article was published.
	    /// </summary>
        [JsonProperty]
        [JsonConverter(typeof(JavaScriptDateTimeConverter))]
	    public DateTimeOffset PublishedDate
	    {
	        get => _publishedDate;
	        set => SetProperty(ref _publishedDate, value);
	    }

        /// <summary>
        /// Gets a formatted version of the feeds published Date
        /// </summary>
        [JsonIgnore]
	    public string PublishedDateFormatted
	    {
	        get
	        {
	            var myCultureInfo = new CultureInfo("de-DE");
	            var myDateTime = DateTime.Parse(PublishedDate.ToString(), myCultureInfo);
	            return myDateTime.ToString("dd.MM.yyyy\thh:mm tt").ToUpper();
	        }
	    }

        /// <summary>
        /// Gets and Sets the article's Date when the article was fetched
        /// </summary>
        [JsonProperty]
        [JsonConverter(typeof(JavaScriptDateTimeConverter))]
	    public DateTimeOffset LastUpdateDate
	    {
	        get => _lastUpdatedTime;
            set => SetProperty(ref _lastUpdatedTime, value);
	    }
        #endregion

        #region helper
        /// <summary>
        ///  Determines whether the specified Feed is equal to the current Feed
        /// </summary>
        /// <param name="feedViewModel">Feed that should be compared to</param>
        /// <returns>True if and only if the Feed has the same id, false otherwise</returns>
	    public override bool Equals(object feedViewModel)
	    {
	        if (!(feedViewModel is FeedViewModel secondFeedViewModel))
	        {
	            return false;
	        }

	        return Id.Equals(secondFeedViewModel.Id);
	    }

        /// <summary>
        /// Gets the hashcode
        /// </summary>
        /// <returns></returns>
	    public override int GetHashCode()
	    {
	        return Id.GetHashCode();
	    }

        /// <summary>
        /// Get or set if the user has watched the article.
        /// </summary>
        [JsonIgnore]
        public bool IsWatched
        {
            get => _isWatched; 
            set => SetProperty(ref _isWatched, value); 
        }
        #endregion
    }
}
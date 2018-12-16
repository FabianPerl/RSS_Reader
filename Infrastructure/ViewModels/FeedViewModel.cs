using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Controls;
using Microsoft.SyndicationFeed;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Prism.Mvvm;

namespace Infrastructure.ViewModels
{
    [JsonObject]
	public class FeedViewModel : BindableBase
	{
	    private const string LinkNoHttpDefined = "Please provide http:// or https://";
	    private const string LinkInvalid = "The Link is invalid";

	    private string _errorMessage = string.Empty;
	    private bool _hasErrors = false;

	    private string _imageUrl;
	    private ICollection<string> _authors;
	    private string _title;
	    private string _shortDescription;

	    private bool _isWatched;

	    private Uri _link;

	    private DateTimeOffset _publishedDate;
	    private DateTimeOffset _lastUpdatedTime;

        #region errors

        [JsonIgnore]
	    public string ErrorMessage
	    {
	        get => _errorMessage;
	        set => SetProperty(ref _errorMessage, value);
	    }

        [JsonIgnore]
	    public bool HasErrors
	    {
	        get => _hasErrors;
	        set => SetProperty(ref _hasErrors, value);
	    }
        #endregion

        #region attributes

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
	        set => SetProperty(ref _shortDescription, value);
	    }

	    /// <summary>
	    /// Get and Set the article's Link to access the feed itself.
	    /// </summary>
        [JsonProperty]
	    public Uri Link
	    {
	        get => _link;
	        set
	        {
	            if (SetProperty(ref _link, value))
                   RaisePropertyChanged(LinkAsString);
	        }
	    }

        [JsonIgnore]
	    public string LinkAsString
	    {
	        get => Link?.OriginalString ?? string.Empty;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    return;

                if (!value.Trim().StartsWith("https://") || !value.Trim().StartsWith("http://"))
                {
                    _errorMessage = LinkNoHttpDefined;
                    return;
                }

                if (Uri.TryCreate(value, UriKind.Absolute, out var uri))
                {
                    if(IsValidFeed(uri))
                        Link = uri;
                }
                else
                {
                    _errorMessage = LinkInvalid;
                }

            }
	    }

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

        [JsonProperty]
        [JsonConverter(typeof(JavaScriptDateTimeConverter))]
	    public DateTimeOffset LastUpdateDate
	    {
	        get => _lastUpdatedTime;
            set => SetProperty(ref _lastUpdatedTime, value);
	    }
        #endregion

        #region helper

	    public override bool Equals(object feedViewModel)
	    {
	        if (!(feedViewModel is FeedViewModel secondFeedViewModel))
	        {
	            return false;
	        }

	        return this.Id.Equals(secondFeedViewModel.Id);
	    }

	    public override int GetHashCode()
	    {
	        return this.Id.GetHashCode();
	    }

	    private bool IsValidFeed(Uri url)
	    {
            //TODO: Better logic
            return true;
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

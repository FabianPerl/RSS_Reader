using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Controls;
using Microsoft.SyndicationFeed;
using Prism.Mvvm;

namespace Infrastructure.ViewModels
{
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
	    public string ErrorMessage
	    {
	        get => _errorMessage;
	        set => SetProperty(ref _errorMessage, value);
	    }

	    public bool HasErrors
	    {
	        get => _hasErrors;
	        set => SetProperty(ref _hasErrors, value);
	    }
        #endregion

        #region attributes
        /// <summary>
        /// Get and Set the article's author(s).
        /// </summary>
        public ICollection<string> Authors
	    {
	        get => _authors;
	        set => SetProperty(ref _authors, value);
	    }

	    /// <summary>
	    /// Get and Set the article's Title.
	    /// </summary>
	    public string Title
	    {
	        get => _title;
	        set => SetProperty(ref _title, value);
	    }

	    /// <summary>
	    /// Get and Set the article's Description. It's a short summary for the article.
	    /// </summary>
	    public string ShortDescription
	    {
	        get => _shortDescription;
	        set => SetProperty(ref _shortDescription, value);
	    }

	    /// <summary>
	    /// Get and Set the article's Link to access the feed itself.
	    /// </summary>
	    public Uri Link
	    {
	        get => _link;
	        set
	        {
	            if (SetProperty(ref _link, value))
                   RaisePropertyChanged(LinkAsString);
	        }
	    }

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

	    public string ImageUrl
	    {
	        get => _imageUrl;
	        set => SetProperty(ref _imageUrl, value);
	    }

	    /// <summary>
	    /// Get and Set the article's Date when the article was published.
	    /// </summary>
	    public DateTimeOffset PublishedDate
	    {
	        get => _publishedDate;
	        set => SetProperty(ref _publishedDate, value);
	    }

	    public string PublishedDateFormatted
	    {
	        get
	        {
	            var myCultureInfo = new CultureInfo("de-DE");
	            var myDateTime = DateTime.Parse(PublishedDate.ToString(), myCultureInfo);
	            return myDateTime.ToString("dd.MM.yyyy\thh:mm tt").ToUpper();
	        }
	    }

	    public DateTimeOffset LastUpdateDate
	    {
	        get => _lastUpdatedTime;
            set => SetProperty(ref _lastUpdatedTime, value);
	    }
        #endregion

        #region helper
        private bool IsValidFeed(Uri url)
	    {
            //TODO: Better logic
            return true;
	    }

        /// <summary>
        /// Get or set if the user has watched the article.
        /// </summary>
        public bool IsWatched
        {
            get => _isWatched; 
            set => SetProperty(ref _isWatched, value); 
        }
        #endregion
    }
}

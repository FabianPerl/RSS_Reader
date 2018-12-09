using System;
using System.Globalization;
using Prism.Mvvm;

namespace Infrastructure.ViewModels
{
	public class FeedViewModel : BindableBase
	{
	    private string _author;
	    private string _title;
	    private string _shortDescription;
	    private bool _isWatched;
	    private Uri _link;
	    private DateTimeOffset _publishedDate;

	    /// <summary>
	    /// Get and Set the article's author(s).
	    /// </summary>
	    public string Author
	    {
	        get => _author;
	        set => SetProperty(ref _author, value);
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
	        set => SetProperty(ref _link, value);
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

        /// <summary>
        /// Get or set if the user has watched the article.
        /// </summary>
        public bool IsWatched
        {
            get => _isWatched; 
            set => SetProperty(ref _isWatched, value); 
        }
	}
}

using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace RSSReader.ViewModels
{
	public class FeedViewModel : BindableBase
	{
        /// <summary>
        /// Get and Set the article's author(s).
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Get and Set the article's Title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Get and Set the article's Description. It's a short summary for the article.
        /// </summary>
        public string ShortDescription { get; set; }

        /// <summary>
        /// Get and Set the article's Link to access the feed itself.
        /// </summary>
        public Uri Link { get; set; }

        /// <summary>
        /// Get and Set the article's Date when the article was published.
        /// </summary>
        public DateTimeOffset PublishedDate { get; set; }

	    public String PublishedDateFormatted
	    {
	        get
	        {
	            CultureInfo myCultureInfo = new CultureInfo("de-DE");
	            DateTime myDateTime = DateTime.Parse(PublishedDate.ToString(), myCultureInfo);
	            return myDateTime.ToString("dd.MM.yyyy\thh:mm tt").ToUpper();
	        }
	    }

	    private bool _isWatched = false;
        /// <summary>
        /// Get or set if the user has watched the article.
        /// </summary>
        public bool IsWatched
        {
            get { return _isWatched; }
            set { SetProperty(ref _isWatched, value); }
        }
	}
}

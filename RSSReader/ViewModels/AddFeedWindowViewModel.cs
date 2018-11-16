using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RSSReader.ViewModels
{

	public class AddFeedWindowViewModel : BindableBase
	{
	    private string _title = "RSS Reader";
	    public string Title
	    {
	        get { return _title; }
	        set { SetProperty(ref _title, value); }
	    }

        public AddFeedWindowViewModel()
        {

        }
	}
}

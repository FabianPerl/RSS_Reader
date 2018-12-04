using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using RSSReader.Models;

namespace RSSReader.ViewModels
{
	public class MainWindowViewModel : BindableBase
	{
	    private readonly SourceList _sourceList = SourceList.GetInstance;

        public MainWindowViewModel()
        {

        }

	    public Collection<Source> AllSources => _sourceList;
    }
}

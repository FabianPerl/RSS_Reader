using Prism.Mvvm;
using System.Collections.ObjectModel;
using RSSReader.Models;

namespace RSSReader.ViewModels
{
	public class MainWindowViewModel : BindableBase
	{
	    private readonly SourceList _sourceList = SourceList.GetInstance;

	    public Collection<Source> AllSources => _sourceList;
    }
}

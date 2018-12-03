using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSSReader.Models
{
    //TODO: Umbauen zum Proxy bzw zu einer Liste mit Singleton source list
    public class SourceList
    {
        public ObservableCollection<Source> AllSources { get; } = new ObservableCollection<Source>();

        private static readonly SourceList Instance = new SourceList();

        private SourceList()
        {
        }

        public static SourceList GetInstance()
        {
            return Instance;
        }
    }
}

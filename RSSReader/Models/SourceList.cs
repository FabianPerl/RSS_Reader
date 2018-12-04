using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace RSSReader.Models
{
    /// <inheritdoc />
    /// <summary>
    /// Singleton SourceList. Can exist only 1 time
    /// </summary>
    public class SourceList : ObservableCollection<Source>
    {
        /// <inheritdoc />
        /// <summary>
        /// private ctor as singleton
        /// </summary>
        private SourceList()
        {
        }

        public static SourceList GetInstance { get; } = new SourceList();
    }
}

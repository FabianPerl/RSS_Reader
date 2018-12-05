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
    public class SingletonList<T> : ObservableCollection<T>
    {
        /// <inheritdoc />
        /// <summary>
        /// private ctor as singleton
        /// </summary>
        private SingletonList()
        {
        }

        public static SingletonList<T> GetInstance { get; } = new SingletonList<T>();
    }
}

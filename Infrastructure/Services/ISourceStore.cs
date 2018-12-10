using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Models;

namespace Infrastructure.Services
{
    public interface ISourceStore
    {
        void SafeSource(Source source);
        void RemoveSource(Source source);
        ICollection<Source> GetAllSources();
    }
}

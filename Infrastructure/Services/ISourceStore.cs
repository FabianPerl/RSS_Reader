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
        ICollection<Source> GetAllSources();
        void DeleteAllSources();
        void SafeAllSources(ICollection<Source> allSources);
    }
}

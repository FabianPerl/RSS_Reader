using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Models;

namespace Infrastructure.Services
{
    public class SourceStoreImpl : ISourceStore
    {
        public void SafeSource(Source source)
        {
            throw new NotImplementedException();
        }

        public void RemoveSource(Source source)
        {
            throw new NotImplementedException();
        }

        public ICollection<Source> GetAllSources()
        {
            throw new NotImplementedException();
        }
    }
}

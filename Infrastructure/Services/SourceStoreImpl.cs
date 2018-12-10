using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Infrastructure.Constants;
using Infrastructure.Models;
using Prism.Logging;

namespace Infrastructure.Services
{
    public class SourceStoreImpl : ISourceStore
    {
        private readonly ILoggerFacade _logger = ProjectLogger.GetLogger;

        public ICollection<Source> GetAllSources()
        {
            _logger.Log("Get all sources", Category.Info, Priority.Medium);
            try
            {
                FileStream fileStream;
                using (fileStream = File.Open(StorePaths.SourceStorePath, FileMode.Open, FileAccess.Read))
                {
                    fileStream.Seek(0, SeekOrigin.Begin);
                    BinaryFormatter serializer = new BinaryFormatter();
                    var sourceList = (ICollection<Source>)serializer.Deserialize(fileStream);
                    fileStream.Close();
                    return sourceList;
                }
            }
            catch (Exception e)
            {
                _logger.Log(e.StackTrace + ", return empty sourcelist", Category.Exception, Priority.High);
                return new ObservableCollection<Source>();
            }
        }

        public void DeleteAllSources()
        {
            _logger.Log("Delete all sources", Category.Info, Priority.Medium);
            SafeAllSources(new ObservableCollection<Source>());
        }

        public void SafeAllSources(ICollection<Source> allSources)
        {
            if (!(File.Exists(StorePaths.SourceStorePath)))
                return;

            try
            {
                using (var fileStream = File.Create(StorePaths.SourceStorePath))
                {
                    fileStream.Seek(0, SeekOrigin.Begin);
                    BinaryFormatter serializer = new BinaryFormatter();
                    serializer.Serialize(fileStream, allSources);
                    fileStream.Close();
                }
            }
            catch (Exception e)
            {
                _logger.Log(e.StackTrace, Category.Exception, Priority.High);
            }
        }

    }
}
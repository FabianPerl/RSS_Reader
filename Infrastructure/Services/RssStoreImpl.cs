using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Infrastructure.Constants;
using Infrastructure.Models;
using Newtonsoft.Json;
using Prism.Logging;
using FeedViewModel = Infrastructure.ViewModels.FeedViewModel;

namespace Infrastructure.Services
{
    public class RssStoreImpl : IRssStore
    {
        private readonly ILoggerFacade _logger = ProjectLogger.GetLogger;

        #region Sources
        public ICollection<Source> GetAllSources()
        {
            _logger.Log("Get all sources", Category.Info, Priority.Medium);
            try
            {
                var jsonSerializer = new JsonSerializer();
                using (var streamReader = new StreamReader(StorePaths.SourceStorePath))
                using (var jsonReader = new JsonTextReader(streamReader))
                {
                    return jsonSerializer.Deserialize<ObservableCollection<Source>>(jsonReader);
                }
            }
            catch (Exception e)
            {
                _logger.Log(e.StackTrace, Category.Exception, Priority.High);
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
            _logger.Log("Safe all Sources", Category.Info, Priority.Medium);
            try
            {
                var jsonSerializer = new JsonSerializer {Formatting = Formatting.Indented};

                using (var streamWriter = new StreamWriter(StorePaths.SourceStorePath))
                using (var jsonWriter = new JsonTextWriter(streamWriter))
                {
                    jsonSerializer.Serialize(jsonWriter, allSources);
                }
            }
            catch (Exception e)
            {
                _logger.Log(e.StackTrace, Category.Exception, Priority.High);
            }
        }
        #endregion

        #region ArchiveFeeds
        public void SafeAllArchiveFeeds(ICollection<FeedViewModel> allFeedViewModels)
        {
            _logger.Log("Safe all Archive Feeds", Category.Info, Priority.Medium);
            try
            {
                var jsonSerializer = new JsonSerializer {Formatting = Formatting.Indented};

                using (var streamWriter = new StreamWriter(StorePaths.ArchivedFeedsStorePath))
                using (var jsonWriter = new JsonTextWriter(streamWriter))
                {
                    jsonSerializer.Serialize(jsonWriter, allFeedViewModels);
                }
            }
            catch (Exception e)
            {
                _logger.Log(e.StackTrace, Category.Exception, Priority.High);
            }
        }

        public ICollection<FeedViewModel> LoadAllArchiveFeeds()
        {
            _logger.Log("Load all Archive Feeds", Category.Info, Priority.Medium);
            try
            {
                var jsonSerializer = new JsonSerializer();
                using (var streamReader = new StreamReader(StorePaths.ArchivedFeedsStorePath))
                using (var jsonReader = new JsonTextReader(streamReader))
                {
                    return jsonSerializer.Deserialize<ObservableCollection<FeedViewModel>>(jsonReader);
                }
            }
            catch (Exception e)
            {
                _logger.Log(e.StackTrace, Category.Exception, Priority.High);
                return new ObservableCollection<FeedViewModel>();
            }
        }
        #endregion
    }
}
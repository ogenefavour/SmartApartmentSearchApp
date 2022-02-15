using Elasticsearch.Net;
using Microsoft.Extensions.Logging;
using Nest;
using Newtonsoft.Json;
using Smart.Apartment.Application.Contracts.Infrastructure;
using Smart.Apartment.Application.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart.Apartment.Infrastructure.Upload
{
    public class UploadService : IUploadService
    {
        ILogger<IUploadService> _logger;
        private readonly IElasticClient _client;
        public UploadService(ILogger<IUploadService> logger, IElasticClient client)
        {
            _logger = logger;
            _client = client;
        }
        public async Task BulkDocumentUpload()
        {
            #region OldImplementation
            //var bytes = File.ReadAllBytes("sample.json");

            //var bulkResponse = await _client.LowLevel.BulkAsync<BulkResponse>(
            //    bytes,
            //    new BulkRequestParameters
            //    {
            //        RequestConfiguration = new RequestConfiguration
            //        {
            //            RequestTimeout = TimeSpan.FromMinutes(3)
            //        }
            //    });

            //if (!bulkResponse.IsValid)
            //{
            //    // handle failure
            //    foreach (var itemWithError in bulkResponse.ItemsWithErrors)
            //    {
            //        _logger.LogInformation($"Failed to upload and index document {itemWithError.Id}: {itemWithError.Error}");
            //    }
            //}
            #endregion

            var management = $"{Path.GetDirectoryName(typeof(IUploadService).Assembly.Location)}\\Upload\\_bulk\\mgmt.json";
            var parseMangements = JsonConvert.DeserializeObject<IEnumerable<ManagementRootModels>>(File.ReadAllText(management));
            var managementindexResponse = new BulkResponse();

            managementindexResponse = await _client.IndexManyAsync(parseMangements.Select(item => item.Mgmt), "management");
            if (!managementindexResponse.IsValid)
                foreach (var itemWithError in managementindexResponse.ItemsWithErrors)
                {
                    _logger.LogInformation($"Failed to upload and index document {itemWithError.Id}: {itemWithError.Error}");
                }
            LogResponse(managementindexResponse);
            var property = $"{Path.GetDirectoryName(typeof(IUploadService).Assembly.Location)}\\Upload\\_bulk\\properties.json";

            var parseProperties = JsonConvert.DeserializeObject<IEnumerable<PropertiesRootModels>>(File.ReadAllText(property));
            var propertiseindexResponse = new BulkResponse();
            propertiseindexResponse = await _client.IndexManyAsync(parseProperties.Select(item => item.Property), "properties");

            LogResponse(propertiseindexResponse);

        }

        private void LogResponse(BulkResponse indexResponse)
        {
            if (!indexResponse.IsValid)
                foreach (var itemWithError in indexResponse.ItemsWithErrors)
                {
                    _logger.LogInformation($"Failed to upload and index document {itemWithError.Id}: {itemWithError.Error}");
                }
        }
    }
}

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Smart.Apartment.Application.Contracts.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Smart.Apartment.UploadService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IUploadService _uploadService;
        public Worker(ILogger<Worker> logger, IUploadService uploadService)
        {
            _logger = logger;
            _uploadService = uploadService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                await _uploadService.BulkDocumentUpload();

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}

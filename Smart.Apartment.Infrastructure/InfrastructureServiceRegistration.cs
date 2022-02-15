using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Smart.Apartment.Application.Contracts.Infrastructure;
using Smart.Apartment.Application.Models.AppSettings;
using Smart.Apartment.Infrastructure.Search;
using Smart.Apartment.Infrastructure.Upload;
using System;
using System.Collections.Generic;
using System.Text;

namespace Smart.Apartment.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            //////services.Configure<EndPoints>(configuration.GetSection("EndPoints"));
            
            services.AddTransient<ISearchService, SearchService>();
            services.AddTransient<IUploadService, UploadService>();

            return services;
        }
    }
}

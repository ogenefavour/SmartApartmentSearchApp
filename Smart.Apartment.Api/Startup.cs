using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Nest;
using Smart.Apartment.Api.Middleware;
using Smart.Apartment.Application;
using Smart.Apartment.Application.Models;
using Smart.Apartment.Application.Models.AppSettings;
using Smart.Apartment.Infrastructure;
using System;

namespace Smart.Apartment.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddControllers();
            services.Configure<Filters>(Configuration.GetSection("Filters"));
            services.AddSingleton<IElasticClient>(sp =>
            {
                var config = sp.GetRequiredService<IConfiguration>();
                var settings = new ConnectionSettings(new Uri(config["EndPoints:ElasticSearchEndpoint"]));
                settings.DefaultIndex("properties");
                settings.DefaultMappingFor<ManagementModel
                    >(s => s.IndexName("management"));
                return new ElasticClient(settings);
            });

            services.AddApplicationServices();
            services.AddInfrastructureServices();

            services.AddSwaggerGen(c =>
            {

                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Smart Apartment Search API",

                });

            });

            services.AddCors(options =>
            {
                options.AddPolicy("Open", builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "GloboTicket Ticket Management API");
            });

            app.UseCustomExceptionHandler();

            app.UseCors("Open");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

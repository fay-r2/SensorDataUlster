using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TodoApi.Models;
using Microsoft.Extensions.Azure;
using Azure.Storage.Queues;
using Azure.Identity;
using System;
using TodoApi.Services;
using System.Text.Json.Serialization;

namespace TodoApi
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
            services.AddScoped<IQueueService, QueueService>();

            services.AddControllers();

            services.AddMvc()
                    .AddJsonOptions(options => {
                        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull ;
            });

            services.AddSwaggerGenNewtonsoftSupport();

            services.AddAzureClients(builder =>
            {
                builder.AddClient<QueueClient, QueueClientOptions>((_, _, _) =>
                {
                    var storageConnectionString = "DefaultEndpointsProtocol=https;AccountName=ulsterassignmentstorage;AccountKey=TcqZuCm+qori7lEn7pybk6WYSyHaxAb87Tf0cH2I8UAdT3AMb3P3HlaPT0Q8ES6HdiXk10d1VLP64MBlBqdBZg==;EndpointSuffix=core.windows.net";
                    var queueName = "sensor-data-queue";
                    return new QueueClient(storageConnectionString, queueName, new QueueClientOptions
                    {
                        MessageEncoding = QueueMessageEncoding.Base64
                    });

                });
            });


                // Register the Swagger generator, defining 1 or more Swagger documents
                services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });

            services.AddDbContext<TodoContext>(options => options.UseInMemoryDatabase("TodoList"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseDefaultFiles();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MySql.Data.MySqlClient;
using PrimeNumbers.NumberAssigner.Core;
using PrimeNumbers.NumberAssigner.Core.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrimeNumbers.NumberAssigner.API
{
    public class Startup
    {
        private ConnectionFactory _connectionFactory;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllers()
                .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PrimeNumbers.NumberAssigner.API", Version = "v1" });
            });

            _connectionFactory = CreateConnectionFactory();
            AssignmentDb assignmentDb = new(_connectionFactory);
            AvailableRangeAssigner availableRangeAssigner = new();
            services.AddSingleton(new AssignmentManager(assignmentDb, availableRangeAssigner));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime applicationLifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PrimeNumbers.NumberAssigner.API v1"));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            applicationLifetime.ApplicationStopping.Register(OnStop);
        }

        private void OnStop()
        {
            _connectionFactory.Dispose();
        }

        private static ConnectionFactory CreateConnectionFactory()
        {
            var connectionParameters = new ConnectionParameters
            {
                Server = "192.168.1.18",
                Port = 3306,
                Database = "numberassignment",
                Username = "root",
                Password = "qwer1234"
            };
            return new ConnectionFactory(connectionParameters);
        }
    }
}

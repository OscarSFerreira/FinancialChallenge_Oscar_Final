using AutoMapper;
using BankRequest.ClientApi.Configuration;
using BuyRequest.Application.Interfaces;
using BuyRequest.Application.Mapping;
using BuyRequest.Application.Services;
using BuyRequest.Data.Context;
using BuyRequest.Data.Repository.BuyRequest;
using BuyRequest.Data.Repository.ProductRequest;
using FinancialChallenge_Oscar.Infrastructure.RabbitMQ.Generic;
using FinancialChallenge_Oscar.Infrastructure.RabbitMQ.Generic.Configuration;
using FinancialChallenge_Oscar.Infrastructure.RabbitMQ.Generic.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

namespace BuyRequestAPI
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
            services.AddControllers().AddJsonOptions(options =>
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BuyRequestAPI", Version = "v1" });
            });
            services.AddDbContext<BuyRequestContext>(cfg =>
            {
                cfg.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                cfg.UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection")).EnableSensitiveDataLogging();
            });
            services.AddBankRequestConfiguration(Configuration);
            //services.AddSingleton<IMessageProducer, RabbitMQProducerGeneric>();
            services.AddScoped<IBuyRequestRepository, BuyRequestRepository>();
            services.AddScoped<IProductRequestRepository, ProductRequestRepository>();
            services.AddScoped<IBuyRequestService, BuyRequestService>();
            services.AddScoped<IProductRequestService, ProductRequestService>();
            services.AddAutoMapper(typeof(Program));
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new BuyRequestMappingProfile());
            });
            var mapper = config.CreateMapper();
            services.AddSingleton(mapper);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BuyRequestAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
using AutoMapper;
using Document.Application.Interfaces;
using Document.Application.Mapping;
using Document.Application.Services;
using Document.Data.Context;
using Document.Data.Repository;
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

namespace DocumentAPI
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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DocumentAPI", Version = "v1" });
            });
            services.AddDbContext<DocumentDataContext>(cfg =>
            {
                cfg.UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddBankRequestConfigurationMQ(Configuration);
            services.AddSingleton<IMessageProducer, RabbitMQProducerGeneric>();
            services.AddScoped<IDocumentRepository, DocumentRepository>();
            services.AddScoped<IDocumentService, DocumentService>();
            services.AddAutoMapper(typeof(Program));
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DocumentMappingProfile());
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
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DocumentAPI v1"));
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
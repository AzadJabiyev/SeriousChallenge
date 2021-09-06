using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Polly;
using Polly.Extensions.Http;
using SeriousChallenge.Infrastructure;
using SeriousChallenge.Infrastructure.Abstractions;
using SeriousChallenge.Infrastructure.Repos;
using SeriousChallengeApi.Controllers;
using SeriousChallengeApi.ControllerServices;
using SeriousChallengeApi.Jobs;
using SeriousChallengeApi.Services;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SeriousChallengeApi
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
            services.AddHttpClient("polygonHttpClient", x =>
             {
                 x.BaseAddress = new Uri("https://api.polygon.io");
             }).AddPolicyHandler(GetRetryPolicy());

            services.AddControllers();

            services.AddDbContext<EFContext>(
                options => options.UseSqlServer(Configuration["ConnectionStrings:SeriousDb"].ToString()));

            services.AddTransient<IStockSymbolRepository, StockSymbolRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IStockSymbolControllerService, StockSymbolControllerService>();
            services.AddSingleton<PolygonService>();

            services.AddHostedService<UpdateSpyData>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var culture = CultureInfo.CreateSpecificCulture("en-US");
            var dateformat = new DateTimeFormatInfo
            {
                ShortDatePattern = "yyyy-MM-dd",
                LongDatePattern = "MM/dd/yyyy hh:mm:ss tt"
            };
            culture.DateTimeFormat = dateformat;

            var supportedCultures = new[] { culture };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(culture),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
              .HandleTransientHttpError()
              .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }
    }
}

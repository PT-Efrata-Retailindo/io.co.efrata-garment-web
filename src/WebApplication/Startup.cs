// Copyright © 2017 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Barebone.IdentityService;
using DanLiris.Admin.Web.Schedulers;
using DanLiris.Admin.Web.Utilities;
using ExtCore.Data.EntityFramework;
using ExtCore.WebApplication.Extensions;
using FluentScheduler;
using IdentityServer4.AccessTokenValidation;
using Infrastructure.External.DanLirisClient.Microservice;
using Infrastructure.External.DanLirisClient.Microservice.Cache;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.ApplicationInsights.AspNetCore;
using Manufactures.Application.AzureUtility;

namespace DanLiris.Admin.Web
{
    public class Startup
    {
        private readonly IConfiguration configuration;
        private readonly string[] EXPOSED_HEADERS = new string[] { "Content-Disposition", "api-version", "content-length", "content-md5", "content-type", "date", "request-id", "response-time" };
        private readonly string extensionsPath;
        private readonly string GARMENT_POLICY = "GarmentPolicy";
        public bool HasAppInsight => !string.IsNullOrEmpty(configuration.GetValue<string>("APPINSIGHTS_INSTRUMENTATIONKEY") ?? configuration.GetValue<string>("ApplicationInsights:InstrumentationKey"));
        public Startup(IHostingEnvironment hostingEnvironment, IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            this.configuration = configuration;
            this.extensionsPath = hostingEnvironment.ContentRootPath + this.configuration["Extensions:Path"];

            loggerFactory.AddConsole();
            loggerFactory.AddDebug();
        }

        public void RegisterMasterDataSettings()
        {
            if (!configuration.GetSection("DanLirisSettings").Exists())
            {
                MasterDataSettings.Endpoint = configuration.GetValue<string>("MasterDataEndpoint") ?? configuration["MasterDataEndpoint"];
                MasterDataSettings.TokenEndpoint = configuration.GetValue<string>("TokenEndpoint") ?? configuration["TokenEndpoint"];
                MasterDataSettings.Username = configuration.GetValue<string>("Username") ?? configuration["Username"];
                MasterDataSettings.Password = configuration.GetValue<string>("Password") ?? configuration["Password"];
            }
            else
            {
                MasterDataSettings.Endpoint = this.configuration.GetSection("DanLirisSettings").GetValue<string>("MasterDataEndpoint");
                MasterDataSettings.TokenEndpoint = this.configuration.GetSection("DanLirisSettings").GetValue<string>("TokenEndpoint");
                MasterDataSettings.Username = this.configuration.GetSection("DanLirisSettings").GetValue<string>("Username");
                MasterDataSettings.Password = this.configuration.GetSection("DanLirisSettings").GetValue<string>("Password");
            }

        }

        public void RegisterPurchasingDataSettings()
        {
            if (!configuration.GetSection("DanLirisSettings").Exists())
            {
                PurchasingDataSettings.Endpoint = configuration.GetValue<string>("PurchasingDataEndpoint") ?? configuration["PurchasingDataEndpoint"];
                PurchasingDataSettings.TokenEndpoint = configuration.GetValue<string>("TokenEndpoint") ?? configuration["TokenEndpoint"];
                PurchasingDataSettings.Username = configuration.GetValue<string>("Username") ?? configuration["Username"];
                PurchasingDataSettings.Password = configuration.GetValue<string>("Password") ?? configuration["Password"];
            }
            else
            {
                PurchasingDataSettings.Endpoint = this.configuration.GetSection("DanLirisSettings").GetValue<string>("PurchasingDataEndpoint");
                PurchasingDataSettings.TokenEndpoint = this.configuration.GetSection("DanLirisSettings").GetValue<string>("TokenEndpoint");
                PurchasingDataSettings.Username = this.configuration.GetSection("DanLirisSettings").GetValue<string>("Username");
                PurchasingDataSettings.Password = this.configuration.GetSection("DanLirisSettings").GetValue<string>("Password");
            }
            
        }

        public void RegisterPackingInventoryDataSettings()
        {
            if (!configuration.GetSection("DanLirisSettings").Exists())
            {
                PackingInventoryDataSettings.Endpoint = configuration.GetValue<string>("PackingInventoryDataEndpoint") ?? configuration["PackingInventoryDataEndpoint"];
                PackingInventoryDataSettings.TokenEndpoint = configuration.GetValue<string>("TokenEndpoint") ?? configuration["TokenEndpoint"];
                PackingInventoryDataSettings.Username = configuration.GetValue<string>("Username") ?? configuration["Username"];
                PackingInventoryDataSettings.Password = configuration.GetValue<string>("Password") ?? configuration["Password"];
            }
            else
            {
                PackingInventoryDataSettings.Endpoint = this.configuration.GetSection("DanLirisSettings").GetValue<string>("PackingInventoryDataEndpoint");
                PackingInventoryDataSettings.TokenEndpoint = this.configuration.GetSection("DanLirisSettings").GetValue<string>("TokenEndpoint");
                PackingInventoryDataSettings.Username = this.configuration.GetSection("DanLirisSettings").GetValue<string>("Username");
                PackingInventoryDataSettings.Password = this.configuration.GetSection("DanLirisSettings").GetValue<string>("Password");
            }
        }

        public void RegisterSalesDataSettings()
		{
			if (!configuration.GetSection("DanLirisSettings").Exists())
			{
				SalesDataSettings.Endpoint = configuration.GetValue<string>("SalesDataEndpoint") ?? configuration["SalesDataEndpoint"];
				SalesDataSettings.TokenEndpoint = configuration.GetValue<string>("TokenEndpoint") ?? configuration["TokenEndpoint"];
				SalesDataSettings.Username = configuration.GetValue<string>("Username") ?? configuration["Username"];
				SalesDataSettings.Password = configuration.GetValue<string>("Password") ?? configuration["Password"];
			}
			else
			{
				SalesDataSettings.Endpoint = this.configuration.GetSection("DanLirisSettings").GetValue<string>("SalesDataEndpoint");
				SalesDataSettings.TokenEndpoint = this.configuration.GetSection("DanLirisSettings").GetValue<string>("TokenEndpoint");
				SalesDataSettings.Username = this.configuration.GetSection("DanLirisSettings").GetValue<string>("Username");
				SalesDataSettings.Password = this.configuration.GetSection("DanLirisSettings").GetValue<string>("Password");
			}

		}

        public void RegisterCustomsDataSettings()
		{
			if (!configuration.GetSection("DanLirisSettings").Exists())
			{
				CustomsDataSettings.Endpoint = configuration.GetValue<string>("CustomsDataEndpoint") ?? configuration["CustomsDataEndpoint"];
                CustomsDataSettings.TokenEndpoint = configuration.GetValue<string>("TokenEndpoint") ?? configuration["TokenEndpoint"];
                CustomsDataSettings.Username = configuration.GetValue<string>("Username") ?? configuration["Username"];
                CustomsDataSettings.Password = configuration.GetValue<string>("Password") ?? configuration["Password"];
			}
			else
			{
                CustomsDataSettings.Endpoint = this.configuration.GetSection("DanLirisSettings").GetValue<string>("CustomsDataEndpoint");
                CustomsDataSettings.TokenEndpoint = this.configuration.GetSection("DanLirisSettings").GetValue<string>("TokenEndpoint");
                CustomsDataSettings.Username = this.configuration.GetSection("DanLirisSettings").GetValue<string>("Username");
                CustomsDataSettings.Password = this.configuration.GetSection("DanLirisSettings").GetValue<string>("Password");
			}

		}

        private void RegisterEndpoint()
        {
            MasterDataSettings.StorageAccountName = this.configuration.GetValue<string>("StorageAccountName") ?? configuration["StorageAccountName"];
            MasterDataSettings.StorageAccountKey = this.configuration.GetValue<string>("StorageAccountKey") ?? configuration["StorageAccountKey"];
        }

        public void ConfigureServices(IServiceCollection services)
        {
            RegisterMasterDataSettings();
            RegisterPurchasingDataSettings();
            RegisterPackingInventoryDataSettings();
            RegisterSalesDataSettings();
            RegisterCustomsDataSettings();
            RegisterEndpoint();
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddTransient<IAzureImage, AzureImage>()
                    .AddTransient<IAzureDocument, AzureDocument>();

            services.AddSingleton<IMemoryCacheManager, MemoryCacheManager>()
                    .AddSingleton<ICoreClient, CoreClient>()
                    .AddSingleton<IPurchasingClient, PurchasingClient>()
                    .AddSingleton<IPackingInventoryClient, PackingInventoryClient>()
                    .AddSingleton<IHttpClientService, HttpClientService>();
            //services.Configure<MasterDataSettings>(options =>
            //{
            //    options.Endpoint = this.configuration.GetSection("DanLirisSettings").GetValue<string>("MasterDataEndpoint");
            //    options.TokenEndpoint = this.configuration.GetSection("DanLirisSettings").GetValue<string>("TokenEndpoint");
            //});

            services.AddExtCore(this.extensionsPath, includingSubpaths: true);

            services.AddMediatR();

            #region Authentication
            string Secret = configuration.GetValue<string>(Constant.SECRET) ?? configuration[Constant.SECRET];
            SymmetricSecurityKey Key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Secret));

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        ValidateLifetime = false,
                        IssuerSigningKey = Key
                    };
                });
            #endregion

            services.Configure<StorageContextOptions>(options =>
            {
                options.ConnectionString = this.configuration.GetConnectionString("Default");
                options.MigrationsAssembly = typeof(DesignTimeStorageContextFactory).GetTypeInfo().Assembly.FullName;
            }
            );

            DesignTimeStorageContextFactory.Initialize(services.BuildServiceProvider());

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info { Title = "Garment API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme { In = "header", Description = "Please enter JWT with Bearer into field", Name = "Authorization", Type = "apiKey" });
                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    { "Bearer", Enumerable.Empty<string>() },
                });

                c.CustomSchemaIds(i => i.FullName);
            });



            #region CORS

            services.AddCors(options => options.AddPolicy(GARMENT_POLICY, builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .WithExposedHeaders(EXPOSED_HEADERS);
            }));

            #endregion

            #region API

            services
                .AddAuthorization();

            #endregion

            #region ApplicationInsight
            services.AddApplicationInsightsTelemetry();
            if (HasAppInsight)
            {
                services.AddApplicationInsightsTelemetry();
                services.AddAppInsightRequestBodyLogging();
            }
            #endregion
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseAuthentication();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }

            if (HasAppInsight)
            {
                app.UseAppInsightRequestBodyLogging();
                app.UseAppInsightResponseBodyLogging();
            }

            app.UseCors(GARMENT_POLICY);
            app.UseExtCore();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
            });

            //JobManager.Initialize(new JobRegistry(app.ApplicationServices));
            JobManager.Initialize(new DefaultScheduleRegistry());
            
        }
    }
}
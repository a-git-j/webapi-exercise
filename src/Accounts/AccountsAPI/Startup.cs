using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using AccountsAPI.Configuration;
using AccountsAPI.Domain.Infrastructure;
using AccountsAPI.Infrastructure;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SimpleWebApi.Core.Data;
using SimpleWebApi.Core.Web.Filters;
using SimpleWebApi.Core.Web.Infrastructure;

namespace AccountsAPI
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvc(x =>
			{
				x.Conventions.Add(new NotFoundResultFilterConvention());
			}).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
			services.ConfigureMvc(null, null);
			services.ConfigureSessionCookie();
			services.ConfigureAntiforgery();
			services.ConfigureApiVersioning();

			services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Fastest);
			services.AddResponseCompression(options => { options.Providers.Add<GzipCompressionProvider>(); });

			services.AddHttpClient();
			services.AddOptions();
			services.Configure<EndpointsConfiguration>(Configuration.GetSection("EndpointsConfiguration"));

			services.AddSingleton<IExceptionResultBuilder, SwaggerExceptionResultBuilder>();
			services.AddSingleton<GlobalExceptionFilter>();
			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			services.AddSingleton<ITestDataProvider, FileDataProvider>();
			services.AddSingleton<IAccountsRepositoryFactory, AccountsInMemRepositoryFactory>();
			services.AddSingleton<ICustomerDetailsRepository, CustomerDetailsRepository>();
			services.AddSingleton<IAccountsTransactionsRepository, TransactionDetailsRepository>();
			


			services.AddSwaggerDocument(settings =>
			{
				settings.Title = "Simple-AccountsAPI";
				settings.Description = "ASPNETCore API built as a demonstration";
				settings.PostProcess = document =>
				{
					document.Info.Version = "v1";
					document.Info.Title = "Accounts API";
					document.Info.Description = "A simple ASPNETCore API for serving customers'a account operations";
					document.Info.TermsOfService = "None";
				};
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, IAntiforgery antiforgery)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				//add security headers
				app.UseSecurityHeadersMiddleware(new SecurityHeadersBuilder()
					.AddDefaultSecureHeadersPolicy());
				//remove headers added by web server
				app.Use(async (context, next) =>
				{
					context.Response.OnStarting(() =>
					{
						context.Response.Headers.Remove(ServerHeader.Name);
						return Task.FromResult(0);
					});
					await next();
				});
				app.UseHttpsRedirection();
				app.UseHsts();
			}
			app.UseStaticFiles();

			//session
			app.UseSession();
			app.Use(async (context, next) =>
			{
				if (context.Request.Path == "/")
				{
					var tokens = antiforgery.GetAndStoreTokens(context);
					context.Response.Cookies.Append("CSRF-TOKEN", tokens.RequestToken, new CookieOptions { HttpOnly = false });
				}
				await next();
			});

			//register swagger 
			app.UseOpenApi();
			app.UseSwaggerUi3(settings =>
			{
				settings.Path = "/docs";
				settings.EnableTryItOut = true;
				settings.DocumentPath = "/docs/swagger.json";
				settings.DocExpansion = "Full";
			});

			app.UseMvc();


			//initialize AutoMapper
			MappingsConfiguration.InitializeAutoMapper();
		}
	}
}

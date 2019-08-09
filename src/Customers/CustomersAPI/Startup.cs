using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CustomersAPI.Domain.Infrastructure;
using CustomersAPI.Infrastructure;
using SimpleWebApi.Core.Data;
using SimpleWebApi.Core.Web.Filters;
using SimpleWebApi.Core.Web.Infrastructure;
using System.Threading.Tasks;
using NSwag.AspNetCore;
using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;
using NLog.Fluent;
using Newtonsoft.Json;

namespace CustomersAPI
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
			services.ConfigureApiVersioning();

			services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Fastest);
			services.AddResponseCompression(options => { options.Providers.Add<GzipCompressionProvider>(); });

			services.AddSingleton<IExceptionResultBuilder, ExceptionResultBuilder>();
			services.AddSingleton<GlobalExceptionFilter>();
			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			services.AddSingleton<ITestDataProvider, FileDataProvider>();
			services.AddSingleton<ICustomersRepositoryFactory, CustomersInMemRepositoryFactory>();
			
			services.AddSwaggerDocument(settings =>
			{
				settings.Title = "Simple-CustomersAPI";
				settings.Description = "ASPNETCore API built as a demonstration";
				settings.PostProcess = document =>
				{
					document.Info.Version = "v1";
					document.Info.Title = "Customers API";
					document.Info.Description = "A simple ASPNETCore API for serving customers information";
					document.Info.TermsOfService = "None";
				};
			});
		}
		

		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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

			app.UseStaticFiles();
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseHsts();
				app.UseHttpsRedirection();
			}

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

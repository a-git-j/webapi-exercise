using System.IO.Compression;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleWebApi.Core.Data;
using SimpleWebApi.Core.Web.Filters;
using SimpleWebApi.Core.Web.Infrastructure;
using TransactionsAPI.Domain.Infrastructure;
using TransactionsAPI.Infrastructure;

namespace TransactionsAPI
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
			services.AddMvc(x =>
			{
				x.Conventions.Add(new NotFoundResultFilterConvention());
			}).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
			services.ConfigureMvc(null, null);
			services.ConfigureSessionCookie();
			services.ConfigureAntiforgery();
			services.ConfigureApiVersioning();

			services.AddSingleton<IExceptionResultBuilder, ExceptionResultBuilder>();
			services.AddSingleton<GlobalExceptionFilter>();
			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			services.AddSingleton<ITestDataProvider, FileDataProvider>();
			services.AddSingleton<ITransactionsRepositoryFactory, TransactionsInMemRepositoryFactory>();
			
			services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Fastest);
			services.AddResponseCompression(options => { options.Providers.Add<GzipCompressionProvider>(); });
			
			services.AddSwaggerDocument(settings =>
			{
				settings.Title = "Simple-TransactionsAPI";
				settings.Description = "ASPNETCore API built as a demonstration";
				settings.PostProcess = document =>
				{
					document.Info.Version = "v1";
					document.Info.Title = "Transactions API";
					document.Info.Description = "A simple ASPNETCore API for serving bank account's transactions";
					document.Info.TermsOfService = "None";
				};
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, IAntiforgery antiforgery)
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

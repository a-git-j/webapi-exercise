using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using SimpleWebApi.Core.Web.Filters;

namespace SimpleWebApi.Core.Web.Infrastructure
{
	public static class ConfigurationExtensions
	{
		public static void ConfigureSessionCookie(this IServiceCollection services)
		{
			services.Configure<CookiePolicyOptions>(options =>
			{
				options.CheckConsentNeeded = context => true;
				options.MinimumSameSitePolicy = SameSiteMode.Strict;
				options.HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always;
				options.Secure = CookieSecurePolicy.SameAsRequest;
			});
			services.AddSession(opts =>
			{
				opts.Cookie.IsEssential = true; // make session cookie essential (no consent needed)
			});			
		}
		public static void ConfigureAntiforgery(this IServiceCollection services)
		{
			services.AddAntiforgery(options =>
			{
				options.HeaderName = "X-CSRF-TOKEN";
			});
		}

		public static void ConfigureApiVersioning(this IServiceCollection services)
		{
			//TODO: implement AreaBasedControllerSelector
		}

		public static void ConfigureMvc(this IServiceCollection services, Action<MvcOptions> configureMvc, Action<MvcJsonOptions> configureJson)
		{			
			services.AddMvcCore(opt =>
			{
				opt.Filters.AddService(typeof(GlobalExceptionFilter));
				opt.ModelValidatorProviders.Clear();
				configureMvc?.Invoke(opt);
			})
			.AddJsonFormatters()
			.AddJsonOptions(opt =>
			{
				configureJson?.Invoke(opt);
			});
			
		}
			   	
	}
}

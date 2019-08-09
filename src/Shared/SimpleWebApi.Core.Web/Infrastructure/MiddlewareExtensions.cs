using Microsoft.AspNetCore.Builder;

namespace SimpleWebApi.Core.Web.Infrastructure
{
	public static class MiddlewareExtensions
	{
		public static IApplicationBuilder UseSecurityHeadersMiddleware(this IApplicationBuilder app, SecurityHeadersBuilder builder)
		{
			SecurityHeadersPolicy policy = builder.Build();
			return app.UseMiddleware<SecurityHeadersMiddleware>(policy);
		}
	}

}

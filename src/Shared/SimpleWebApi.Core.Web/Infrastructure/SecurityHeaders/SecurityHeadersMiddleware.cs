using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SimpleWebApi.Core.Web.Infrastructure
{
	public class SecurityHeadersMiddleware
	{
		
		public SecurityHeadersMiddleware(RequestDelegate next, SecurityHeadersPolicy policy)
		{
			_next = next;
			_policy = policy;
		}
		private readonly RequestDelegate _next;
		private readonly SecurityHeadersPolicy _policy;


		public async Task Invoke(HttpContext context)
		{
			IHeaderDictionary headers = context.Response.Headers;

			foreach (var header in _policy.SetHeaders)
			{
				headers[header.Key] = header.Value;
			}

			foreach (var header in _policy.RemoveHeaders)
			{
				headers.Remove(header);
			}

			await _next(context);
		}
	}
}

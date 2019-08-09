using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SimpleWebApi.Core.Web.Models.Errors;

namespace SimpleWebApi.Core.Web.Filters
{
	public class ExceptionResultBuilder : IExceptionResultBuilder
	{
		protected readonly ILogger<ExceptionResultBuilder> _logger;

		public ExceptionResultBuilder(ILogger<ExceptionResultBuilder> logger)
		{
			_logger = logger;
		}

		public virtual IActionResult Build(Exception exception)
		{
			var statusCode = 500;
			string content = null;
			var message = exception.Message;

			if (exception is SimpleWebApiBaseException customException)
			{
				statusCode = (int)customException.StatusCode;
				content = customException.GetContent();				
			}
			return CreateActionResult(content, message, statusCode, exception);
		}

		
		protected virtual IActionResult CreateActionResult(string content, string message, int statusCode, Exception exception)
		{
			var objectResult = new ObjectResult(content ?? message)
			{
				StatusCode = statusCode
			};
			_logger.LogError(exception, message);
			return objectResult;
		}
	}
}

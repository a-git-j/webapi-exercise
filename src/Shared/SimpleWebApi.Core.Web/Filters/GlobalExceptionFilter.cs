using Microsoft.AspNetCore.Mvc.Filters;

namespace SimpleWebApi.Core.Web.Filters
{
	public class GlobalExceptionFilter : IExceptionFilter
	{
		private readonly IExceptionResultBuilder _exceptionResultBuilder;

		public GlobalExceptionFilter(IExceptionResultBuilder exceptionResultBuilder)
		{
			_exceptionResultBuilder = exceptionResultBuilder;
		}

		public void OnException(ExceptionContext context)
		{
			var exception = context.Exception;
						
			context.Result = _exceptionResultBuilder.Build(exception); 
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountsAPI.Infrastructure.Services.Customers;
using AccountsAPI.Infrastructure.Services.Transactions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SimpleWebApi.Core.Web.Filters;
using SimpleWebApi.Core.Web.Models.Errors;

namespace AccountsAPI.Infrastructure
{
	public class SwaggerExceptionResultBuilder : ExceptionResultBuilder
	{
		
		public SwaggerExceptionResultBuilder(ILogger<ExceptionResultBuilder> logger)
			:base(logger)
		{
			
		}

		public override IActionResult Build(Exception exception)
		{
			var statusCode = 500;
			string content = null;
			var message = exception.Message;

			if (exception is SimpleWebApiBaseException customException)
			{
				statusCode = (int)customException.StatusCode;
				content = customException.GetContent();
			}
			else if (exception is SwaggerCustomerClientException customersException) 
			{
				statusCode = customersException.StatusCode;
				content = customersException.Message;
			}
			else if (exception is SwaggerTransactionClientException transactionsException)
			{
				statusCode = transactionsException.StatusCode;
				content = transactionsException.Message;
			}
			return CreateActionResult(content, message, statusCode, exception);
		}

	}
}

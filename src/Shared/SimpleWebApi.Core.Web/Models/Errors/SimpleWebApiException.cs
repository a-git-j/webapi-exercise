using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace SimpleWebApi.Core.Web.Models.Errors
{
	public class SimpleWebApiException : SimpleWebApiBaseException
	{
		public SimpleWebApiException(HttpStatusCode statusCode, string message)
			: base(statusCode, message, null)
		{

		}

		public override string GetContent()
		{
			return Message;
		}
	}
}

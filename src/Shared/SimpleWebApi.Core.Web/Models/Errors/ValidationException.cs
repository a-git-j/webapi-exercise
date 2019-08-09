using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace SimpleWebApi.Core.Web.Models.Errors
{
	public class ValidationException : SimpleWebApiException<IEnumerable<string>>
	{
		public ValidationException(string message, IEnumerable<string> validationErrors)
			: base(HttpStatusCode.BadRequest, message, validationErrors ?? new List<string>())
		{
		}

		public override string GetContent()
		{
			return JsonConvert.SerializeObject(Content);
		}
	}
}

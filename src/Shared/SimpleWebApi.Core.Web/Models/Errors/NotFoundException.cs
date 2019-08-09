using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace SimpleWebApi.Core.Web.Models.Errors
{
	public class NotFoundException : SimpleWebApiBaseException
	{
		public NotFoundException(string message)
			: base(HttpStatusCode.NotFound, message, null)
		{

		}
				
		public override string GetContent()
		{
			return "";
		}
	}
}

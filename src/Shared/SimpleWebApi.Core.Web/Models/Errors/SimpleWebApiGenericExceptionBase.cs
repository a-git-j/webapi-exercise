using System;
using System.Net;
using Newtonsoft.Json;

namespace SimpleWebApi.Core.Web.Models.Errors
{
	public class SimpleWebApiException<TContent> : SimpleWebApiBaseException, IWebApiException<TContent>
	{
		public SimpleWebApiException(string message, TContent content = default(TContent)) 
			: this(HttpStatusCode.BadRequest, message, null, content)
		{
		}

		public SimpleWebApiException(HttpStatusCode statusCode, string message, TContent content = default(TContent)) 
			: this(statusCode, message, null, content)
		{
		}

		public SimpleWebApiException(HttpStatusCode statusCode, string message, Exception innerException, TContent content = default(TContent)) 
			: base(statusCode, message, innerException, content)
		{
		}

		public TContent Content
		{
			get => (TContent)InternalContent;
			set => InternalContent = value;
		}

		public override string GetContent()
		{
			if (Content != null)
			{
				var body = JsonConvert.SerializeObject(Content);
				return body;
			}

			return null;
		}
	}
}

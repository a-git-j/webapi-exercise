using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace SimpleWebApi.Core.Web.Models.Errors
{
	public abstract class SimpleWebApiBaseException : Exception
	{		
		protected SimpleWebApiBaseException(HttpStatusCode statusCode, string message, Exception innerException, object content = null) 
			: base(message, innerException)
		{
			StatusCode = statusCode;
			InternalContent = content;
		}

		protected SimpleWebApiBaseException(string message, object content = null)
			: this(HttpStatusCode.BadRequest, message, null, content)
		{
		}

		protected SimpleWebApiBaseException(HttpStatusCode statusCode, string message, object content = null)
			: this(statusCode, message, null, content)
		{
		}


		public HttpStatusCode StatusCode { get; set; }
		protected object InternalContent { get; set; }

		public abstract string GetContent();
		public virtual object GetRawContent()
		{
			return InternalContent;
		}
	}
}

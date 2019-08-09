using System.Net;

namespace SimpleWebApi.Core.Web.Models.Errors
{
	interface IWebApiException<TContent>
	{
		HttpStatusCode StatusCode { get; set; }
		TContent Content { get; set; }
	}
}

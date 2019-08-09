using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SimpleWebApi.Core.Web.Models.Errors;

namespace SimpleWebApi.Core.Web.Filters
{
	public class NotFoundResultFilterAttribute : ResultFilterAttribute
	{
		public override void OnResultExecuting(ResultExecutingContext context)
		{
			if (context.Result is ObjectResult objectResult && objectResult.Value == null)
			{
				throw new NotFoundException("Resource not found");
			}
			else
			{
				base.OnResultExecuting(context);
			}
		}
	}
}

using System;
using Microsoft.AspNetCore.Mvc;

namespace SimpleWebApi.Core.Web.Filters
{
	public interface IExceptionResultBuilder
	{
		IActionResult Build(Exception exception);
	}
}

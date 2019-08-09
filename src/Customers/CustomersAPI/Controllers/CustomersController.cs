using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CustomersAPI.Domain.Models;
using CustomersAPI.Domain.Infrastructure;
using SimpleWebApi.Core.Web.Models.Errors;
using NSwag.Annotations;
using System.Net;

namespace CustomersAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CustomersController : ControllerBase
	{
		public CustomersController(ICustomersRepositoryFactory repositoryFactory)
		{
			_repoFactory = repositoryFactory;
		}
		private readonly ICustomersRepositoryFactory _repoFactory;

		/// <summary>
		///		Gets information about existing customer
		/// </summary>
		/// <param name="customerId">Customer identifier</param>
		/// <returns></returns>
		[HttpGet("{customerId}")]
		[SwaggerResponse(HttpStatusCode.OK, typeof(CustomerViewModel))]
		[SwaggerResponse(HttpStatusCode.BadRequest, typeof(Exception))]
		public async Task<ActionResult<CustomerViewModel>> Get(int customerId)
		{
			if (ModelState.IsValid)
			{
				var customer = await _repoFactory.CreateRepository().GetCustomerAsync(customerId);
				return Ok(customer);
			}
			else
			{
				throw new ValidationException("Please provide customerId first!", null);
			}
		}
				

	}
}

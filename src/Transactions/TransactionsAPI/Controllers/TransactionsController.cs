using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using SimpleWebApi.Core.Web.Models.Errors;
using TransactionsAPI.Domain.Infrastructure;
using TransactionsAPI.Domain.Models;

namespace TransactionsAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TransactionsController : ControllerBase
	{
		public TransactionsController(ITransactionsRepositoryFactory factory)
		{
			_repoFactory = factory;
		}
		private readonly ITransactionsRepositoryFactory _repoFactory;

		/// <summary>
		///		Lists all transactions associated with the account
		/// </summary>
		/// <param name="accountId">Account number</param>
		/// <returns></returns>
		[HttpGet("{accountId}")]
		[SwaggerResponse(HttpStatusCode.OK, typeof(IEnumerable<TransactionViewModel>))]
		[SwaggerResponse(HttpStatusCode.BadRequest, typeof(Exception))]
		public async Task<ActionResult<IEnumerable<TransactionViewModel>>> Get(int accountId)
		{
			if (ModelState.IsValid)
			{
				var transactions = await _repoFactory.CreateRepository().GetTransactionsForAccountAsync(accountId);
				return Ok(transactions);
			}
			else
			{
				throw new ValidationException("Please provide accountId first!", null);
			}
		}

		/// <summary>
		///		Registers transaction against given account
		/// </summary>
		/// <param name="transaction">Account number and transaction amount specification</param>
		/// <returns></returns>
		[HttpPost]
		[ActionName("New")]
		[SwaggerResponse(HttpStatusCode.OK, typeof(string))]
		[SwaggerResponse(HttpStatusCode.BadRequest, typeof(Exception))]

		public async Task<ActionResult> Post([FromBody] TransactionViewModel transaction)
		{
			if (ModelState.IsValid && transaction.Amount != 0d)
			{
				var saved = await _repoFactory.CreateRepository().SaveTransactionAsync(transaction.AccountId, transaction.Amount);
				if (saved == null)
				{
					return StatusCode(500);
				}
				return Ok("Transaction registered");
			}
			else
			{
				throw new ValidationException("Please provide valid transactions details first!", null);
			}
		}
		
	}
}

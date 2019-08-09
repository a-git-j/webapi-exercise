using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using SimpleWebApi.Core.Web.Models.Errors;
using AccountsAPI.Domain.Infrastructure;
using AccountsAPI.Domain.Models;
using AccountsAPI.Configuration;
using Microsoft.Extensions.Options;
using System;

namespace TransactionsAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountsController : ControllerBase
	{
		private readonly IAccountsRepositoryFactory _repoFactory;
		private readonly ICustomerDetailsRepository _repoCustomer;
		private readonly IAccountsTransactionsRepository _repoTransactions;
		private readonly IOptions<EndpointsConfiguration> _config;

		public AccountsController(IAccountsRepositoryFactory factory, ICustomerDetailsRepository customers, IAccountsTransactionsRepository transactions, IOptions<EndpointsConfiguration> config)
		{
			_config = config;
			_repoFactory = factory;
			_repoCustomer = customers;
			_repoTransactions = transactions;
		}
		
		/// <summary>
		///		Registers new account for existing customer
		/// </summary>
		/// <param name="account">Customer information and inital payload to accoun</param>
		/// <returns>Returns customer's new account id</returns>
		[HttpPost]
		[ActionName("New")]
		[SwaggerResponse(HttpStatusCode.Created, typeof(int))]
		[SwaggerResponse(HttpStatusCode.BadRequest, typeof(Exception))]
		[SwaggerResponse(HttpStatusCode.Conflict, typeof(Exception))]
		public async Task<ActionResult> Post([FromBody] NewAccount account)
		{
			if (ModelState.IsValid)
			{
				//check customer exists
				var customer = await _repoCustomer.GetCustomerInfoAsync(_config.Value.CustomerBaseUrl, account.CustomerId);
				if (customer == null)
				{
					throw new NotFoundException(string.Format("Customer with id '{0}' not found!", account.CustomerId));
				}
								
				//create account
				var created = await _repoFactory.CreateRepository().CreateAccountAsync(account.CustomerId);
				if (created == null)
				{
					throw new SimpleWebApiException(HttpStatusCode.InternalServerError, "Unable to create account for customer!");
				}

				var responseUrl = string.Format(@"{0}/{1}", _config.Value.CustomerDetailsUrl, account.CustomerId);

				//check initial credit
				if (!account.InitialCredit.HasValue || account.InitialCredit.Value == 0d)
				{
					return Created(responseUrl, created.AccountId);
				}
				//register initial credit and wait for transaction status
				await MakeInitialCreditTransaction(created.AccountId, account.InitialCredit.Value);
				return Created(responseUrl, created.AccountId);
			}
			else
			{
				throw new ValidationException("Please provide valid cusotmer id!", null);
			}
		}

		private async Task<string> MakeInitialCreditTransaction(int accountId, double initialCredit)
		{
			try
			{
				var trans = await _repoTransactions.MakeTransactionAsync(_config.Value.TransactionBaseUrl, accountId, initialCredit);
				return trans;
			}
			catch(Exception)
			{
				var rollback = await RollbackAccountCreation(accountId);
				var message = rollback ? "Account not created due to error upon initial credit registering." : "Unable to register initial credit! Account creation could not be rollbacked.";
				throw new SimpleWebApiException(HttpStatusCode.InternalServerError, message);
			}
		}

		private async Task<bool> RollbackAccountCreation(int accountId)
		{
			try
			{
				var deleted  = await _repoFactory.CreateRepository().RemoveAccountAsync(accountId);
				return deleted;
			}
			catch (Exception)
			{
				return false;
			}
		}

	}
}

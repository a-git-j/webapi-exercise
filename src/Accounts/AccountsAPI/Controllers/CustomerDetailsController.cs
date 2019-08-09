using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using SimpleWebApi.Core.Web.Models.Errors;
using AccountsAPI.Domain.Infrastructure;
using AccountsAPI.Domain.Models;
using Microsoft.Extensions.Options;
using AccountsAPI.Configuration;
using System.Linq;
using System;

namespace TransactionsAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CustomerDetailsController : ControllerBase
	{
		private readonly IAccountsRepositoryFactory _repoFactory;
		private readonly ICustomerDetailsRepository _repoCustomer;
		private readonly IAccountsTransactionsRepository _repoTransactions;
		private readonly IOptions<EndpointsConfiguration> _config;

		public CustomerDetailsController(IAccountsRepositoryFactory factory, ICustomerDetailsRepository customers, IAccountsTransactionsRepository transactions, IOptions<EndpointsConfiguration> config)
		{
			_config = config;
			_repoFactory = factory;
			_repoCustomer = customers;
			_repoTransactions = transactions;
		}

		/// <summary>
		///		Lists all accounts with associated transactions for the customer
		/// </summary>
		/// <param name="customerId">Customer number</param>
		/// <returns></returns>
		[HttpGet("{customerId}")]
		[SwaggerResponse(HttpStatusCode.OK, typeof(CustomerDetailsViewModel))]
		[SwaggerResponse(HttpStatusCode.BadRequest, typeof(Exception))]
		public async Task<ActionResult<CustomerDetailsViewModel>> GetDetailsAsync(int customerId)
		{
			if (ModelState.IsValid)
			{
				//check customer exists
				var customer = await GetCustomerInfo(customerId);

				//get all accounts for customer
				var accounts = await GetAccounts(customerId, true);

				//get transactions for accounts
				if (accounts != null)
				{
					foreach (var acct in accounts)
					{
						var transactions = await GetTransactions(acct.AccountId, true);
						acct.Transactions = transactions?.Select(x => x.Amount);
						acct.CurrentBalance = acct.Transactions?.Sum() ?? 0;
					}
				}

				//model
				var details = new CustomerDetailsViewModel
				{
					Name = customer.Name,
					Surname = customer.Surname,
					Accounts = accounts
				};
				return Ok(details);
			}
			else
			{
				throw new ValidationException("Please provide customerId first!", null);
			}
		}

		private async Task<CustomerInfo> GetCustomerInfo(int customerId)
		{
			var customer = await _repoCustomer.GetCustomerInfoAsync(_config.Value.CustomerBaseUrl, customerId);
			if (customer == null)
			{
				throw new ValidationException("Please provide valid cusotmer id!", null);
			}
			return customer;
		}
		private async Task<IEnumerable<AccountViewModel>> GetAccounts(int customerId, bool errorSilentMode)
		{
			IEnumerable<AccountViewModel> accounts = null;
			try
			{
				accounts = await _repoFactory.CreateRepository().GetCustomerAccountsAsync(customerId);
			}
			catch (SimpleWebApiBaseException)
			{
				if (!errorSilentMode) throw;
			}
			return accounts;
		}
		private async Task<IEnumerable<TransactionInfo>> GetTransactions(int accountId, bool errorSilentMode)
		{
			IEnumerable<TransactionInfo> transactions = null;
			try
			{
				transactions = await _repoTransactions.GetTransactionDetailsAsync(_config.Value.TransactionBaseUrl, accountId);
			}
			catch (SimpleWebApiBaseException)
			{
				if (!errorSilentMode) throw;
			}
			return transactions;
		}

	}

}


using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AccountsAPI.Domain.Infrastructure;
using AccountsAPI.Domain.Models;
using AccountsAPI.Infrastructure.Services.Transactions;

namespace AccountsAPI.Infrastructure
{
	public class TransactionDetailsRepository : IAccountsTransactionsRepository
	{
		private readonly IHttpClientFactory _httpClientFactory;

		public TransactionDetailsRepository(IHttpClientFactory clientFactory)
		{
			_httpClientFactory = clientFactory;
		}

		public async Task<IEnumerable<TransactionInfo>> GetTransactionDetailsAsync(string transactionServiceUrl, int accountId)
		{
			var client = new TransactionsClient(transactionServiceUrl, _httpClientFactory.CreateClient());

			var transactions = await client.GetAsync(accountId);

			return AutoMapper.Mapper.Map<IEnumerable<TransactionViewModel>, IEnumerable<TransactionInfo>>(transactions);
		}

		public async Task<string> MakeTransactionAsync(string transactionServiceUrl, int accountId, double amount)
		{
			var client = new TransactionsClient(transactionServiceUrl, _httpClientFactory.CreateClient());

			var transactionInfo = await client.NewAsync(new TransactionViewModel { AccountId = accountId, Amount = amount });

			return transactionInfo;
		}
	}
}

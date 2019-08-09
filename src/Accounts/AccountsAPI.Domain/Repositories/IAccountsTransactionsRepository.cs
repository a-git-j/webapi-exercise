using System.Collections.Generic;
using System.Threading.Tasks;
using AccountsAPI.Domain.Models;

namespace AccountsAPI.Domain.Infrastructure
{
	public interface IAccountsTransactionsRepository
	{
		Task<string> MakeTransactionAsync(string transactionServiceUrl, int accountId, double amount);
		Task<IEnumerable<TransactionInfo>> GetTransactionDetailsAsync(string transactionServiceUrl, int accountId);
	}
}

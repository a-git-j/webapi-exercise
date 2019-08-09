using System.Collections.Generic;
using System.Threading.Tasks;
using TransactionsAPI.Domain.Models;

namespace TransactionsAPI.Domain.Infrastructure
{
	public interface ITransactionsRepository
	{
		Task<TransactionViewModel> SaveTransactionAsync(int accountId, double amount);
		Task<IEnumerable<TransactionViewModel>> GetTransactionsForAccountAsync(int accountId);

		void ClearData();
	}
}

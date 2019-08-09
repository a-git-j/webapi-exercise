using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using TransactionsAPI.Domain.Infrastructure;
using TransactionsAPI.Domain.Models;
using TransactionsAPI.Infrastructure.Models;
using SimpleWebApi.Core.Data;
using System.Collections.Generic;
using System.Threading;

namespace TransactionsAPI.Infrastructure
{
	public class TransactionsInMemRepository : ITransactionsRepository
	{
		public TransactionsInMemRepository(ITestDataProvider dataProvider)
		{
			_dataProvider = dataProvider;
			InitializeDataStore();
		}
		private readonly ITestDataProvider _dataProvider;
		private ConcurrentBag<Transaction> _data;
		
		private void InitializeDataStore()
		{
			if (_data == null || !_data.Any())
			{
				_data = _dataProvider?.ReadTestData<Transaction>() ?? new ConcurrentBag<Transaction>();
			}			
		}

		public async Task<IEnumerable<TransactionViewModel>> GetTransactionsForAccountAsync(int accountId)
		{
			var transactions = new List<TransactionViewModel>();

			//mock asynchronous reading from data source - only for exercise purposes!
			await Task.Factory.StartNew(() =>
			{
				var data = _data.Where(x => x.AccountId == accountId).ToList();

				foreach (var transaction in data)
				{
					transactions.Add(AutoMapper.Mapper.Map<TransactionViewModel>(transaction));
				}
			});

			if (transactions.Any())
			{
				return transactions;
			}			
			return null;
		}

		public async Task<TransactionViewModel> SaveTransactionAsync(int accountId, double amount)
		{
			TransactionViewModel newTransaction = null;
			//mock asynchronous data saving to data source - only for exercise purposes!
			await Task.Factory.StartNew(() =>
			{
				var transaction = new Transaction { AccountId = accountId, Amount = amount };
				_data.Add(transaction);
				newTransaction = AutoMapper.Mapper.Map<TransactionViewModel>(transaction);
			});

			return newTransaction;
		}

		public void ClearData()
		{
			_data.Clear();
		}
	}
}

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccountsAPI.Domain.Infrastructure;
using AccountsAPI.Domain.Models;
using AccountsAPI.Infrastructure.Models;
using SimpleWebApi.Core.Data;

namespace AccountsAPI.Infrastructure
{
	public class AccountsInMemRepository : IAccountsRepository
	{
		public AccountsInMemRepository(ITestDataProvider dataProvider)
		{
			_dataProvider = dataProvider;
			InitializeDataStore();
		}
		private readonly ITestDataProvider _dataProvider;
		private ConcurrentDictionary<int, Account> _data;

		private int _currentSeqId = 0;
		private readonly object _lockSeq = new object();

		private void InitializeDataStore()
		{
			if (_data == null || !_data.Any())
			{
				_data = _dataProvider?.ReadKeyedTestData<Account>() ?? new ConcurrentDictionary<int, Account>();
				_currentSeqId = _data.Any() ? _data.Keys.Max() : 0;
			}
		}

		public async Task<AccountViewModel> CreateAccountAsync(int customerId)
		{
			AccountViewModel newAccount = null;

			await Task.Factory.StartNew(() =>
			{
				var account = CreateWithLocking(customerId);
				newAccount = AutoMapper.Mapper.Map<Account, AccountViewModel>(account);
			});

			return newAccount;
		}
		public async Task<bool> RemoveAccountAsync(int accountId)
		{
			bool deleted = false;

			await Task.Factory.StartNew(() =>
			{
				deleted = _data.TryRemove(accountId, out Account value);
			});

			return deleted;
		}
		private Account CreateWithoutLocking(int customerId)
		{
			//non-concurrent 
			var acct = new Account { Id = ++_currentSeqId, CustomerId = customerId };
			if (_data.TryAdd(acct.Id, acct))
			{
				return acct;
			}
			return null;
		}
		private Account CreateWithLocking(int customerId)
		{
			lock (_lockSeq)
			{
				var acct = new Account { Id = _currentSeqId + 1, CustomerId = customerId };
				if (_data.TryAdd(acct.Id, acct))
				{
					_currentSeqId = +1;
					return acct;
				}
				else
				{
					return null;
				}
			}
		}

		public async Task<IEnumerable<AccountViewModel>> GetCustomerAccountsAsync(int customerId)
		{
			var accounts = new List<AccountViewModel>();

			//mock asynchronous reading from data source - only for exercise purposes!
			await Task.Factory.StartNew(() =>
			{
				var filtered = _data.Where(x => x.Value.CustomerId == customerId);

				foreach (var account in filtered)
				{
					accounts.Add(AutoMapper.Mapper.Map<AccountViewModel>(account.Value));
				}
			});

			if (accounts.Any())
			{
				return accounts;
			}
			return null;
		}
	}
}

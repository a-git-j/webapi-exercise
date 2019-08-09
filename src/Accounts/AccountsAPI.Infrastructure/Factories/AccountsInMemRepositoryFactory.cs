using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using AccountsAPI.Domain.Infrastructure;
using SimpleWebApi.Core.Data;

namespace AccountsAPI.Infrastructure
{
	public class AccountsInMemRepositoryFactory : IAccountsRepositoryFactory
	{

		private readonly ITestDataProvider _testDataProvider;
		private IAccountsRepository _accountRepository;
		
		public AccountsInMemRepositoryFactory(ITestDataProvider dataProvider)
		{
			_testDataProvider = dataProvider;
		}

		public IAccountsRepository CreateRepository()
		{			
			//since it's in-memory repo we want to initialize instance only once
			//note: no need for singleton here: DI is going to handle that
			if (_accountRepository == null)
			{
				_accountRepository = new AccountsInMemRepository(_testDataProvider);
			}
			return _accountRepository;
		}

		
	}
}

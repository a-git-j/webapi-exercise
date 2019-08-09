using TransactionsAPI.Domain.Infrastructure;
using SimpleWebApi.Core.Data;

namespace TransactionsAPI.Infrastructure
{
	public class TransactionsInMemRepositoryFactory : ITransactionsRepositoryFactory
	{
		public TransactionsInMemRepositoryFactory(ITestDataProvider dataProvider)
		{
			_testDataProvider = dataProvider;
		}
		private readonly ITestDataProvider _testDataProvider;
		private ITransactionsRepository _repository;

		public ITransactionsRepository CreateRepository()
		{
			//since it's in-memory repo we want to initialize instance only once
			//note: no need for singleton here: DI is going to handle that
			if (_repository  == null)
			{
				_repository = new TransactionsInMemRepository(_testDataProvider);
			}
			return _repository;
		}
	}
}

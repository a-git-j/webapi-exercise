using CustomersAPI.Domain.Infrastructure;
using SimpleWebApi.Core.Data;

namespace CustomersAPI.Infrastructure
{
	public class CustomersInMemRepositoryFactory : ICustomersRepositoryFactory
	{
		public CustomersInMemRepositoryFactory(ITestDataProvider dataProvider)
		{
			_testDataProvider = dataProvider;
		}
		private readonly ITestDataProvider _testDataProvider;
		private ICustomersRepository _repository;

		public ICustomersRepository CreateRepository()
		{
			//since it's in-memory repo we want to initialize instance only once
			//note: no need for singleton here: DI is going to handle that
			if (_repository  == null)
			{
				_repository = new CustomersInMemRepository(_testDataProvider);
			}
			return _repository;
		}
	}
}

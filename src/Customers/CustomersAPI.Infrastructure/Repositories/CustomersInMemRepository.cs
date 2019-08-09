using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using CustomersAPI.Domain.Infrastructure;
using CustomersAPI.Domain.Models;
using CustomersAPI.Infrastructure.Models;
using SimpleWebApi.Core.Data;

namespace CustomersAPI.Infrastructure
{
	public class CustomersInMemRepository : ICustomersRepository
	{
		public CustomersInMemRepository(ITestDataProvider dataProvider)
		{
			_dataProvider = dataProvider;
			InitializeDataStore();
		}
		private readonly ITestDataProvider _dataProvider;
		private ConcurrentDictionary<int, Customer> _data;
		
		private void InitializeDataStore()
		{
			if (_data == null || !_data.Any())
			{
				_data = _dataProvider?.ReadKeyedTestData<Customer>() ?? new ConcurrentDictionary<int, Customer>();
			}
		}


		public async Task<CustomerViewModel> GetCustomerAsync(int customerId)
		{
			bool succeeded = false;
			Customer customer = null;

			//mock asynchronous reading from data source - only for exercise purposes!
			await Task.Factory.StartNew(() =>
			{
				succeeded = _data.TryGetValue(customerId, out customer);
			});

			if (succeeded)
			{
				return AutoMapper.Mapper.Map<CustomerViewModel>(customer);
			}
			return null;
		}

	}
}

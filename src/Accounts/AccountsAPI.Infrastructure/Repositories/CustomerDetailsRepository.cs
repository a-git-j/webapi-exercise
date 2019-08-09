using System.Net.Http;
using System.Threading.Tasks;
using AccountsAPI.Domain.Infrastructure;
using AccountsAPI.Domain.Models;
using AccountsAPI.Infrastructure.Services.Customers;

namespace AccountsAPI.Infrastructure
{
	public class CustomerDetailsRepository : ICustomerDetailsRepository
	{
		private readonly IHttpClientFactory _httpClientFactory;

		public CustomerDetailsRepository(IHttpClientFactory clientFactory)
		{
			_httpClientFactory = clientFactory;
		}
		
		public async Task<CustomerInfo> GetCustomerInfoAsync(string customerServiceUrl, int customerId)
		{
			var client = new CustomersClient(customerServiceUrl, _httpClientFactory.CreateClient());

			//if customer does not exist -> 404
			var customerInfo = await client.GetAsync(customerId);

			return new CustomerInfo
			{
				Name = customerInfo.Name,
				Surname = customerInfo.Surname
			};
		}


		
	}
}

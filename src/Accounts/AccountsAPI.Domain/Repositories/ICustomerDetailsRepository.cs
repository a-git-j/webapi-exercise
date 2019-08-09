using System.Threading.Tasks;
using AccountsAPI.Domain.Models;

namespace AccountsAPI.Domain.Infrastructure
{
	public interface ICustomerDetailsRepository
	{
		Task<CustomerInfo> GetCustomerInfoAsync(string customerServiceUrl, int customerId);
	}
}

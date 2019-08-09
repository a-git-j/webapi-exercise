namespace CustomersAPI.Domain.Infrastructure
{
	public interface ICustomersRepositoryFactory
	{
		ICustomersRepository CreateRepository();
	}
}

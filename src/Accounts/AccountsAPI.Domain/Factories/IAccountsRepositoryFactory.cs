namespace AccountsAPI.Domain.Infrastructure
{
	public interface IAccountsRepositoryFactory
	{
		IAccountsRepository CreateRepository();
	}
}

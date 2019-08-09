namespace TransactionsAPI.Domain.Infrastructure
{
	public interface ITransactionsRepositoryFactory
	{
		ITransactionsRepository CreateRepository();
	}
}

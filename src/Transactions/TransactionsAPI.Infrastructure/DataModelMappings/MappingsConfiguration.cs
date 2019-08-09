using AutoMapper;
using TransactionsAPI.Domain.Models;
using TransactionsAPI.Infrastructure.Models;

namespace TransactionsAPI.Infrastructure
{
	public static class MappingsConfiguration
	{
		/// <summary>
		/// Initialize AutoMapper (static) configuration - overkill for such a small domain model; using it though for demonstration purpose
		/// </summary>
		public static void InitializeAutoMapper()
		{
			Mapper.Initialize(x =>
			{
				x.CreateMap<Transaction, TransactionViewModel>();
			});
		}
	}
}

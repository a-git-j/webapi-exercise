using AutoMapper;
using AccountsAPI.Domain.Models;
using AccountsAPI.Infrastructure.Models;
using AccountsAPI.Infrastructure.Services.Transactions;
using System.Collections.Generic;
using TransactionInfo = AccountsAPI.Domain.Models.TransactionInfo;

namespace AccountsAPI.Infrastructure
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
				x.CreateMap<Account, AccountViewModel>()
					.ForMember(dest => dest.AccountId, opt => opt.MapFrom((src, dest) => src.Id))
					.ForMember(dest => dest.CurrentBalance, opt => opt.Ignore())
					.ForMember(dest => dest.Transactions, opt => opt.Ignore());

				x.CreateMap<TransactionViewModel, TransactionInfo>();
				x.CreateMap<List<TransactionViewModel>, List<TransactionInfo>>();
			});


		}
	}
}

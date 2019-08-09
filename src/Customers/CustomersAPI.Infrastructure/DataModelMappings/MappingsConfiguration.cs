using AutoMapper;
using CustomersAPI.Domain.Models;
using CustomersAPI.Infrastructure.Models;

namespace CustomersAPI.Infrastructure
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
				x.CreateMap<Customer, CustomerViewModel>();
			});
		}
	}
}

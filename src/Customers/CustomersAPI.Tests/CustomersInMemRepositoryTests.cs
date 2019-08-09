using System.Collections.Concurrent;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using CustomersAPI.Domain.Models;
using CustomersAPI.Infrastructure.Models;
using SimpleWebApi.Core.Data;

namespace CustomersAPI.Infrastructure
{
	public class CustomersInMemRepositoryTests
	{
		[OneTimeSetUp]
		public void Setup()
		{
			_dataProvider = new Mock<ITestDataProvider>();
			_dataProvider.Setup(x => x.ReadKeyedTestData<Customer>()).Returns(() =>
			{
				var dict = new ConcurrentDictionary<int, Customer>();
				dict.GetOrAdd(1, new Customer { Id = 1, Name = "c1", Surname = "s1" });
				dict.GetOrAdd(2, new Customer { Id = 2, Name = "c2", Surname = "s2" });
				return dict;
			});
		}
		private Mock<ITestDataProvider> _dataProvider;

		[Test]
		public async Task GetCustomer_ForNonExistingCustomerId_ReturnsNullAsync()
		{
			var repo = new CustomersInMemRepository(_dataProvider.Object);

			var result = await repo.GetCustomerAsync(666);

			result.Should().BeNull();
		}
		[Test]
		public void GetCustomer_ForExistingCustomerId_ReturnsCustomerView()
		{
			var repo = new CustomersInMemRepository(_dataProvider.Object);
			MappingsConfiguration.InitializeAutoMapper();

			Task<CustomerViewModel> task = repo.GetCustomerAsync(1);
			var result = task.Result;

			result.Name.Should().Be("c1");
			result.Surname.Should().Be("s1");
		}
	}
}

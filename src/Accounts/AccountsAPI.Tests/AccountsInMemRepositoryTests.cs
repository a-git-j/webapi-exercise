using NUnit.Framework;
using FluentAssertions;
using Moq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Concurrent;
using AccountsAPI.Infrastructure.Models;
using AccountsAPI.Domain.Models;
using SimpleWebApi.Core.Data;

namespace AccountsAPI.Infrastructure
{
	public class AccountsInMemRepositoryTests
	{
		[OneTimeSetUp]
		public void Setup()
		{
			_dataProvider = new Mock<ITestDataProvider>();
			_dataProvider.Setup(x => x.ReadKeyedTestData<Account>()).Returns(() =>
			{
				var dict = new ConcurrentDictionary<int, Account>();
				dict.GetOrAdd(1, new Account { Id = 1, CustomerId = 1 });
				dict.GetOrAdd(2, new Account { Id = 2, CustomerId = 1 });
				dict.GetOrAdd(3, new Account { Id = 3, CustomerId = 1 });
				dict.GetOrAdd(4, new Account { Id = 4, CustomerId = 2 });
				dict.GetOrAdd(5, new Account { Id = 5, CustomerId = 2 });
				return dict;
			});

			MappingsConfiguration.InitializeAutoMapper();
		}
		private Mock<ITestDataProvider> _dataProvider;

		[Test]
		public async Task GetCustomerAccountsAsync_ForNonExistingAccountId_ReturnsNullAsync()
		{
			var repo = new AccountsInMemRepository(_dataProvider.Object);

			var result = await repo.GetCustomerAccountsAsync(100);

			result.Should().BeNull();
		}
		[Test]
		public void GetCustomerAccountsAsync_ForExistingAccountId_ReturnsNonEmptyList()
		{
			var repo = new AccountsInMemRepository(_dataProvider.Object);
			
			Task<IEnumerable<AccountViewModel>> task = repo.GetCustomerAccountsAsync(1);
			var result = task.Result;

			result.Should().NotBeNullOrEmpty();
		}
		[Test]
		public void GetCustomerAccountAsync_WithNoAccountData_ReturnsNull()
		{
			var dataProvider = new Mock<ITestDataProvider>();
			var repo = new AccountsInMemRepository(dataProvider.Object);

			Task<IEnumerable<AccountViewModel>> task = repo.GetCustomerAccountsAsync(1);
			var result = task.Result;

			result.Should().BeNull();
		}
		
		[Test]
		public void CreateAccountsAsync_ForExistingCustomerId_CreatesAccount()
		{
			var repo = new AccountsInMemRepository(_dataProvider.Object);
			
			Task<AccountViewModel> task = repo.CreateAccountAsync(3);
			var result = task.Result;

			result.Should().NotBeNull();
		}
		[Test]
		public void CreateAccountAsync_WithNoAccountData_ReturnsNewFirstAccount()
		{
			var dataProvider = new Mock<ITestDataProvider>();
			var repo = new AccountsInMemRepository(dataProvider.Object);

			Task<AccountViewModel> task = repo.CreateAccountAsync(1);
			var result = task.Result;

			result.Should().NotBeNull();
			result.AccountId.Should().Be(1);
		}
	}
}
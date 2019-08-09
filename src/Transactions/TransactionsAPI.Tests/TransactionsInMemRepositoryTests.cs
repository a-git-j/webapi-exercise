using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SimpleWebApi.Core.Data;
using TransactionsAPI.Infrastructure.Models;
using TransactionsAPI.Domain.Models;

namespace TransactionsAPI.Infrastructure
{
	public class TransactionsInMemRepositoryTests
	{
		[OneTimeSetUp]
		public void Setup()
		{
			_dataProvider = new Mock<ITestDataProvider>();
			_dataProvider.Setup(x => x.ReadTestData<Transaction>()).Returns(() =>
			{
				var dict = new ConcurrentBag<Transaction>();
				dict.Add(new Transaction { AccountId = 1, Amount = 666 });
				dict.Add(new Transaction { AccountId = 2, Amount = -666 });
				return dict;
			});
		}
		private Mock<ITestDataProvider> _dataProvider;

		[Test]
		public async Task GetTransactionsForAccountAsync_ForNonExistingAccountId_ReturnsNullAsync()
		{
			var repo = new TransactionsInMemRepository(_dataProvider.Object);

			var result = await repo.GetTransactionsForAccountAsync(100);

			result.Should().BeNull();
		}
		[Test]
		public void GetTransactionsForAccountAsync_ForExistingAccountId_ReturnsNonEmptyList()
		{
			var repo = new TransactionsInMemRepository(_dataProvider.Object);
			MappingsConfiguration.InitializeAutoMapper();
			
			Task<IEnumerable<TransactionViewModel>> task = repo.GetTransactionsForAccountAsync(1);
			var result = task.Result;

			result.Should().NotBeNullOrEmpty();
			result.Should().OnlyContain( x => x.AccountId == 1);
		}
	}
}

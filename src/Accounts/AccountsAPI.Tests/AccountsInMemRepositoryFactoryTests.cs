using NUnit.Framework;
using FluentAssertions;
using Moq;
using SimpleWebApi.Core.Data;
using System.Net.Http;

namespace AccountsAPI.Infrastructure
{
	public class AccountsInMemRepositoryFactoryTests
	{
		[OneTimeSetUp]
		public void Setup()
		{
			_dataProvider = new Mock<ITestDataProvider>();
		}
		private Mock<ITestDataProvider> _dataProvider;
		[Test]
		public void CreateAccountsRepository_ReturnsInMemRepoInstance()
		{
			var factory = new AccountsInMemRepositoryFactory(_dataProvider.Object);

			var result = factory.CreateRepository();

			result.Should().BeOfType(typeof(AccountsInMemRepository));
		}
		[Test]
		public void CreateAccountsRepository_CalledTwice_ReturnsSameInMemRepoInstance()
		{
			var factory = new AccountsInMemRepositoryFactory(_dataProvider.Object);

			var result = factory.CreateRepository();
			var result2 = factory.CreateRepository();

			result2.Should().Be(result);
		}


	}
}
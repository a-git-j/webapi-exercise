using FluentAssertions;
using Moq;
using NUnit.Framework;
using SimpleWebApi.Core.Data;

namespace TransactionsAPI.Infrastructure
{
	public class TransactionsInMemRepositoryFactoryTests
	{
		[OneTimeSetUp]
		public void Setup()
		{
			_dataProvider = new Mock<ITestDataProvider>();
		}
		private Mock<ITestDataProvider> _dataProvider;
		[Test]
		public void CreateRepository_ReturnsInMemRepoInstance()
		{
			var factory = new TransactionsInMemRepositoryFactory(_dataProvider.Object);

			var result = factory.CreateRepository();

			result.Should().BeOfType(typeof(TransactionsInMemRepository));
		}
		[Test]
		public void CreateRepository_CalledTwice_ReturnsSameInMemRepoInstance()
		{
			var factory = new TransactionsInMemRepositoryFactory(_dataProvider.Object);

			var result = factory.CreateRepository();
			var result2 = factory.CreateRepository();

			result2.Should().Be(result);
		}
	}
}

using System;
using System.Collections.Generic;
using System.Text;
using CustomersAPI.Infrastructure;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SimpleWebApi.Core.Data;

namespace CustomersAPI.Infrastructure
{
	public class CustomersInMemRepositoryFactoryTests
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
			var factory = new CustomersInMemRepositoryFactory(_dataProvider.Object);

			var result = factory.CreateRepository();

			result.Should().BeOfType(typeof(CustomersInMemRepository));
		}
		[Test]
		public void CreateRepository_CalledTwice_ReturnsSameInMemRepoInstance()
		{
			var factory = new CustomersInMemRepositoryFactory(_dataProvider.Object);

			var result = factory.CreateRepository();
			var result2 = factory.CreateRepository();

			result2.Should().Be(result);
		}
	}
}

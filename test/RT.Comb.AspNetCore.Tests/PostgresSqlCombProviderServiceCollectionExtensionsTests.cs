﻿using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using Xunit;

namespace RT.Comb.AspNetCore.Tests {

	public sealed class PostgresSqlCombProviderServiceCollectionExtensionsTests {

		[Fact]
		public void AddPostgreSqlCombGuidWithSqlDateTime_ShouldAddPostgreSqlProviderWithDefaultStrategies() {
			// Arrange
			var services = new ServiceCollection();

			// Act
			services.AddPostgreSqlCombGuidWithSqlDateTime();

			// Assert
			var p = services.BuildServiceProvider().GetService<ICombProvider>();
			Assert.NotNull(p);
			Assert.IsType<SqlDateTimeStrategy>(TestUtils.GetCurrentDateTimeStrategy(p));
		}

		[Fact]
		public void AddPostgreSqlCombGuidWithSqlDateTime_CustomStrategies_CreateShouldInvokeCustomStrategies() {
			// Arrange
			var services = new ServiceCollection();

			// custom timestamp and guid strategies
			var customTimestampProviderMock = new Mock<TimestampProvider>();
			customTimestampProviderMock.Setup(p => p.Invoke()).Returns(DateTime.Now);

			var customGuidProviderMock = new Mock<GuidProvider>();
			customGuidProviderMock.Setup(p => p.Invoke()).Returns(Guid.NewGuid());

			services.AddPostgreSqlCombGuidWithSqlDateTime(customTimestampProviderMock.Object, customGuidProviderMock.Object);
			var comb = services.BuildServiceProvider().GetService<ICombProvider>();

			// Act
			var _ = comb.Create();

			// Assert
			customTimestampProviderMock.Verify(p => p.Invoke(), Times.Once);
			customGuidProviderMock.Verify(p => p.Invoke(), Times.Once);
		}

		[Fact]
		public void AddPostgreSqlCombGuidWithUnixDateTime_ShouldAddPostgreSqlProviderWithDefaultStrategies() {
			// Arrange
			var services = new ServiceCollection();

			// Act
			services.AddPostgreSqlCombGuidWithUnixDateTime();

			// Assert
			var p = services.BuildServiceProvider().GetService<ICombProvider>();
			Assert.NotNull(p);
			Assert.IsType<UnixDateTimeStrategy>(TestUtils.GetCurrentDateTimeStrategy(p));
		}

		[Fact]
		public void AddPostgreSqlCombGuidWithUnixDateTime_CustomStrategies_CreateShouldInvokeCustomStrategies() {
			// Arrange
			var services = new ServiceCollection();

			// custom timestamp and guid strategies
			var customTimestampProviderMock = new Mock<TimestampProvider>();
			customTimestampProviderMock.Setup(p => p.Invoke()).Returns(DateTime.Now);

			var customGuidProviderMock = new Mock<GuidProvider>();
			customGuidProviderMock.Setup(p => p.Invoke()).Returns(Guid.NewGuid());

			services.AddPostgreSqlCombGuidWithUnixDateTime(customTimestampProviderMock.Object, customGuidProviderMock.Object);
			var serviceProvider = services.BuildServiceProvider();
			var comb = serviceProvider.GetService<ICombProvider>();

			// Act
			var _ = comb.Create();

			// Assert
			customTimestampProviderMock.Verify(p => p.Invoke(), Times.Once);
			customGuidProviderMock.Verify(p => p.Invoke(), Times.Once);
		}

	}

}

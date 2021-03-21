using DevBlog.Application.Extensions;
using System;
using Xunit;

namespace DevBlog.Application.UnitTests.Extensions
{
	public class DateTimeExtensionsTests
	{
		[Fact]
		public void ToDayWithFullMonthAndYear_WhenCalledOnDateTime_ReturnsFormattedDateString()
		{
			// Arrange
			var dt = new DateTime(2019, 10, 22, 8, 8, 8, 8);

			// Act
			var result = dt.ToDayWithFullMonthAndYear();

			// Assert
			Assert.Equal("22nd October 2019", result);
		}

	}
}
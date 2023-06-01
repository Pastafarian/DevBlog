using DevBlog.Application.Extensions;
using Xunit;

namespace DevBlog.Application.UnitTests.Extensions
{
	public class StringExtensionsTests
	{
		[Fact]
		public void TruncateText_WhenPassedNullString_ThenReturnsEmptyString()
		{
			// Arrange
			const string testString = null;

			// Act + Assert
			Assert.Equal(string.Empty, testString.TruncateText(50));
		}

		[Fact]
		public void TruncateText_WhenStringShorterThanLimit_ThenReturnsSameString()
		{
			// Arrange
			const string testString = "This string is less than 50 characters.";

			// Act + Assert
			Assert.Equal(testString, testString.TruncateText(50));
		}

		[Fact]
		public void TruncateText_WhenContainsSpace_ThenCutsOnSpace()
		{
			// Arrange
			const string testString = "fff fff";

			// Act
			var result = testString.TruncateText(5, false);

			//Assert
			Assert.Equal("fff", result);
		}

		[Fact]
		public void TruncateText_WhenEllipsisSpecifiedOnStringWithSpace_ThenEllipsisAdded()
		{
			// Arrange
			const string testString = "fff fff";

			// Act
			var result = testString.TruncateText(6);

			// Assert
			Assert.Equal("fff...", result);
		}

		[Fact]
		public void TruncateText_WhenEllipsisSpecifiedOnTextWithNoSpace_ThenEllipsisAdded()
		{
			// Arrange
			const string testString = "ffffff";

			// Act
			var result = testString.TruncateText(3);

			// Assert
			Assert.Equal("fff...", result);
		}


		[Fact]
		public void TruncateFilename_ReturnsNullWhenGivenNullFilename()
		{
			// Arrange
			const string filename = null;
			const int maxLength = 200;

			// Act
			var result = filename.TruncateFilename(maxLength);

			// Assert
			Assert.Equal(filename, result);
		}

		[Fact]
		public void TruncateFilename_ReturnsUnchangedFilenameWhenUnderMaxLength()
		{
			// Arrange
			const string filename = "foobar.baz";
			const int maxLength = 200;

			// Act
			var result = filename.TruncateFilename(maxLength);

			// Assert
			Assert.Equal(filename, result);
		}

		[Fact]
		public void TruncateFilename_GivenFilenameExceedsMaxLength_ThenShorternsName()
		{
			// Arrange
			const string filename = "foobarfoobar.baz";
			const int maxLength = 10;

			// Act
			var result = filename.TruncateFilename(maxLength);

			// Assert
			Assert.Equal("foobar.baz", result);
		}

	}
}

using LogsQuerer.Query;

namespace LogsQuerer.Tests.Query
{
    public class QueryParseExtensionsTests
    {
        [Fact]
        public void TrimQuotes_ShouldTrimQuotes()
        {
            // Arrange
            string str = "'test'";
            // Act
            var result = str.TrimQuotes();
            // Assert
            Assert.Equal("test", result);
        }

        [Fact]
        public void TrimQuotes_ShouldTrimDoubleQuotes()
        {
            // Arrange
            string str = "\"test\"";
            // Act
            var result = str.TrimQuotes();
            // Assert
            Assert.Equal("test", result);
        }

        [Fact]
        public void TrimQuotes_ShouldTrimBackticks()
        {
            // Arrange
            string str = "`test`";
            // Act
            var result = str.TrimQuotes();
            // Assert
            Assert.Equal("test", result);
        }

        [Fact]
        public void TrimQuotes_ShouldTrimOneBacktick()
        {
            // Arrange
            string str = "`test";
            // Act
            var result = str.TrimQuotes();
            // Assert
            Assert.Equal("test", result);
        }

        [Fact]
        public void TrimQuotes_ShouldTrimManyBackticks()
        {
            // Arrange
            string str = "````test``";
            // Act
            var result = str.TrimQuotes();
            // Assert
            Assert.Equal("test", result);
        }
    }
}

using LogsQuerer.Query;

namespace LogsQuerer.Tests.Query
{
    public class SqlLikeQueryParserTests
    {
        public static IEnumerable<object?[]> GetSqlLikeQueryData()
        {
            yield return new object[] {
                "select FROM column_name2 WHERE text = 'search_string'",
                new QueryNode[]
                {
                    new QueryNode { Not = false, Operator = QueryOperator.None, Column = "column_name2", Value = "search_string" }
                }
            };
            yield return new object[] {
                "SELECT   from   'column_name2'   where   not   text=\"search_string\"  ",
                new QueryNode[]
                {
                    new QueryNode { Not = true, Operator = QueryOperator.None, Column = "column_name2", Value = "search_string" }
                }
            };
            yield return new object[] {
                "select FROM column_name WHERE not text='search_string' OR text=\"search_string2\"",
                new QueryNode[]
                {
                    new QueryNode { Not = true, Operator = QueryOperator.None, Column = "column_name", Value = "search_string" },
                    new QueryNode { Not = false, Operator = QueryOperator.Or, Column = "column_name", Value = "search_string2" }
                }
            };
            yield return new object[] {
                "select FROM column_name WHERE text='search_string' or text=\"search_string2\"",
                new QueryNode[]
                {
                    new QueryNode { Not = false, Operator = QueryOperator.None, Column = "column_name", Value = "search_string" },
                    new QueryNode { Not = false, Operator = QueryOperator.Or, Column = "column_name", Value = "search_string2" }
                }
            };
            yield return new object[] {
                "select FROM column_name WHERE text='search_string' AND NOT text=\"search_string2\" or text=\"search_string3\" or text = \"search_string4\" and text=\"search_string5\"",
                new QueryNode[]
                {
                    new QueryNode { Not = false, Operator = QueryOperator.None, Column = "column_name", Value = "search_string" },
                    new QueryNode { Not = true, Operator = QueryOperator.And, Column = "column_name", Value = "search_string2" },
                    new QueryNode { Not = false, Operator = QueryOperator.Or, Column = "column_name", Value = "search_string3" },
                    new QueryNode { Not = false, Operator = QueryOperator.Or, Column = "column_name", Value = "search_string4" },
                    new QueryNode { Not = false, Operator = QueryOperator.And, Column = "column_name", Value = "search_string5" },
                }
            };
            yield return new object?[] {
                "select * FROM column_name WHERE text='search_string'",
                null
            };
            yield return new object?[] {
                "select FROM column_name",
                null
            };
            yield return new object?[] {
                "select",
                null
            };
            yield return new object?[] {
                "",
                null
            };
            yield return new object?[] {
                "column_name = 'search_string'",
                null
            };
        }

        [Theory]
        [MemberData(nameof(GetSqlLikeQueryData), MemberType = typeof(SqlLikeQueryParserTests))]
        public void TryParse_ReturnsCorrectResults(string query, QueryNode[] expectedNodes)
        {
            // Arrange
            var parser = new SqlLikeQueryParser();

            // Act
            var result = parser.TryParse(query, out var queryNodes);

            // Assert
            Assert.Equal(expectedNodes != null, result);
            Assert.Equivalent(expectedNodes, queryNodes);
        }
    }
}
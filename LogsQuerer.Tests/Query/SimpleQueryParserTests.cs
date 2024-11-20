using LogsQuerer.Query;

namespace LogsQuerer.Tests.Query
{
    public class SimpleQueryParserTests
    {
        public static IEnumerable<object?[]> GetSimpleQueryData()
        {
            yield return new object[] {
                "   column_name2         =   'search_string' ",
                new QueryNode[]
                {
                    new QueryNode { Not = false, Operator = QueryOperator.None, Column = "column_name2", Value = "search_string" }
                }
            };
            yield return new object[] {
                "NOT column_name2=\"search_string\"  ",
                new QueryNode[]
                {
                    new QueryNode { Not = true, Operator = QueryOperator.None, Column = "column_name2", Value = "search_string" }
                }
            };
            yield return new object[] {
                "not column_name='search_string'  OR   column_name2=\"search_string2\"",
                new QueryNode[]
                {
                    new QueryNode { Not = true, Operator = QueryOperator.None, Column = "column_name", Value = "search_string" },
                    new QueryNode { Not = false, Operator = QueryOperator.Or, Column = "column_name2", Value = "search_string2" }
                }
            };
            yield return new object[] {
                "column_name='search_string' or column_name=\"search_string2\"",
                new QueryNode[]
                {
                    new QueryNode { Not = false, Operator = QueryOperator.None, Column = "column_name", Value = "search_string" },
                    new QueryNode { Not = false, Operator = QueryOperator.Or, Column = "column_name", Value = "search_string2" }
                }
            };
            yield return new object[] {
                "column_name='search_string' AND NOT column_name2=\"search_string2\" or column_name3=\"search_string3\" or column_name4 = \"search_string4\" and column_name5=\"search_string5\"",
                new QueryNode[]
                {
                    new QueryNode { Not = false, Operator = QueryOperator.None, Column = "column_name", Value = "search_string" },
                    new QueryNode { Not = true, Operator = QueryOperator.And, Column = "column_name2", Value = "search_string2" },
                    new QueryNode { Not = false, Operator = QueryOperator.Or, Column = "column_name3", Value = "search_string3" },
                    new QueryNode { Not = false, Operator = QueryOperator.Or, Column = "column_name4", Value = "search_string4" },
                    new QueryNode { Not = false, Operator = QueryOperator.And, Column = "column_name5", Value = "search_string5" },
                }
            };
            yield return new object?[] {
                "severity=4",
                new QueryNode[]
                {
                    new QueryNode { Not = false, Operator = QueryOperator.None, Column = "severity", Value = "4" },
                }
            };
            yield return new object?[] {
                "select FROM column_name2 WHERE text = 'search_string'",
                null
            };
            yield return new object?[] {
                "   select  * FROM column_name WHERE text='search_string'",
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
        }

        [Theory]
        [MemberData(nameof(GetSimpleQueryData), MemberType = typeof(SimpleQueryParserTests))]
        public void TryParse_ReturnsCorrectResults(string query, QueryNode[] expectedNodes)
        {
            // Arrange
            var parser = new SimpleQueryParser();

            // Act
            var result = parser.TryParse(query, out var queryNodes);

            // Assert
            Assert.Equal(expectedNodes != null, result);
            Assert.Equivalent(expectedNodes, queryNodes);
        }
    }
}

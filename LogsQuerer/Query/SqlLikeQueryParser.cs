using System.Text.RegularExpressions;

namespace LogsQuerer.Query
{
    public class SqlLikeQueryParser : ISqlLikeQueryParser
    {
        public bool TryParse(string query, out QueryNode[]? queryNodes)
        {
            queryNodes = null;

            var pattern = @"select\s+FROM\s+(?<column>[\w""']+)\s+WHERE\s+(?<conditions>(not\s+)?text\s*=\s*('([^']*)'|""([^""]*)"")(\s*(and|or)\s*(not\s+)?text\s*=\s*('([^']*)'|""([^""]*)""))*)\s*";

            var match = Regex.Match(query, pattern, RegexOptions.IgnoreCase);

            if (!match.Success)
            {
                return false;
            }

            var column = match.Groups["column"].Value.TrimQuotes();
            var conditions = match.Groups["conditions"].Value;

            var conditionPattern = @"\s*(?<operator>and|or)?\s*(?<not>not\s+)?text\s*=\s*('(?<value1>[^']*)'|""(?<value2>[^""]*)"")\s*";
            var conditionMatches = Regex.Matches(conditions, conditionPattern, RegexOptions.IgnoreCase);

            var nodes = new List<QueryNode>();

            foreach (Match conditionMatch in conditionMatches)
            {
                var not = conditionMatch.Groups["not"].Success;
                var value = conditionMatch.Groups["value1"].Success ? conditionMatch.Groups["value1"].Value : conditionMatch.Groups["value2"].Value;
                var operatorStr = conditionMatch.Groups["operator"].Value.Trim().ToLower();

                var queryOperator = operatorStr switch
                {
                    "and" => QueryOperator.And,
                    "or" => QueryOperator.Or,
                    _ => QueryOperator.None
                };

                nodes.Add(new QueryNode
                {
                    Not = not,
                    Operator = queryOperator,
                    Column = column,
                    Value = value
                });
            }

            queryNodes = nodes.ToArray();
            return true;
        }
    }
}

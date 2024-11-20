using System.Text.RegularExpressions;

namespace LogsQuerer.Query
{
    public class SimpleQueryParser : ISimpleQueryParser
    {
        public bool TryParse(string query, out QueryNode[]? queryNodes)
        {
            queryNodes = null;

            if (Regex.IsMatch(query, @"^\s*select.*FROM.*", RegexOptions.IgnoreCase))
            {
                return false;
            }

            var pattern = @"\s*(?<operator>and\s+|or\s+)?(?<not>not\s+)?(?<column>[\w""']+)\s*=\s*('(?<value1>[^']*)'|""(?<value2>[^""]*)""|(?<value3>\w+))\s*";
            var matches = Regex.Matches(query, pattern, RegexOptions.IgnoreCase);

            if (matches.Count == 0)
            {
                return false;
            }

            var nodes = new List<QueryNode>();

            foreach (Match match in matches)
            {
                var not = match.Groups["not"].Success;
                var column = match.Groups["column"].Value.TrimQuotes();
                var value = match.Groups["value1"].Success ? match.Groups["value1"].Value : (match.Groups["value2"].Success ? match.Groups["value2"].Value : match.Groups["value3"].Value);
                var operatorStr = match.Groups["operator"].Value.Trim().ToLower();

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

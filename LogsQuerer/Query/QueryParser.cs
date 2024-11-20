namespace LogsQuerer.Query
{
    public class QueryParser(ISimpleQueryParser simpleQueryParser, ISqlLikeQueryParser sqlLikeQueryParser) : IQueryParser
    {
        public bool TryParse(string query, out QueryNode[]? queryNodes)
        {
            if (sqlLikeQueryParser.TryParse(query, out queryNodes))
            {
                return true;
            }

            if (simpleQueryParser.TryParse(query, out queryNodes))
            {
                return true;
            }

            queryNodes = null;
            return false;
        }
    }
}

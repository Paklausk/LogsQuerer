namespace LogsQuerer.Query
{
    public class QueryNode
    {
        public bool Not { get; set; }
        public QueryOperator Operator { get; set; }
        public string? Column { get; set; }
        public string? Value { get; set; }
    }
}

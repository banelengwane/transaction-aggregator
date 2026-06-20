namespace TransactionAggregator.Models
{
    /* 
        this handles the standardized schema that the API 
        promises to return
     */
    public class Transaction
    {
        public string Id { get; set; } = string.Empty;
        public string SourceBank { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "ZAR";
        public string Category { get; set; } = "Uncategorized";
        public DateTime TransactionDate { get; set; }
        public string Description { get; set; } = string.Empty;
    }

    public class StandardizedResponse<T>
    {
        public bool Success { get; set; } = true;
        public T Data { get; set; } = default;
        public PaginationMetaData? Pagination { get; set; }
    }

    public class PaginationMetaData
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
    }
}
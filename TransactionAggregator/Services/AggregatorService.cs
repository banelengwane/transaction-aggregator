// File: Services/AggregatorService.cs
using TransactionAggregator.Models;

namespace TransactionAggregator.Services
{
    public interface IAggregatorService
    {
        Task<(IEnumerable<Transaction> Data, int TotalCount)> GetTransactionsAsync(
            DateTime? startDate, DateTime? endDate, string? category, int page, int pageSize);
    }

    public class AggregatorService : IAggregatorService
    {
        private readonly IEnumerable<IBankProvider> _bankProviders;

        public AggregatorService(IEnumerable<IBankProvider> bankProviders)
        {
            _bankProviders = bankProviders;
        }

        public async Task<(IEnumerable<Transaction> Data, int TotalCount)> GetTransactionsAsync(
            DateTime? startDate, DateTime? endDate, string? category, int page, int pageSize)
        {
            // Fire off all requests concurrently
            var tasks = _bankProviders.Select(p => p.FetchTransactionsAsync());
            var results = await Task.WhenAll(tasks);
            
            // Flatten the results into a single collection
            var allTransactions = results.SelectMany(t => t);

            // Filtering by Date Range
            if (startDate.HasValue)
                allTransactions = allTransactions.Where(t => t.TransactionDate >= startDate.Value);

            if (endDate.HasValue)
                allTransactions = allTransactions.Where(t => t.TransactionDate <= endDate.Value);

            // Filtering by Category
            if (!string.IsNullOrEmpty(category))
                allTransactions = allTransactions.Where(t => t.Category.Equals(category, StringComparison.OrdinalIgnoreCase));

            // Default Sort: Latest transactions first
            allTransactions = allTransactions.OrderByDescending(t => t.TransactionDate);

            int totalCount = allTransactions.Count();

            // Apply Pagination formulas
            var paginatedData = allTransactions
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return (paginatedData, totalCount);
        }
    }
}
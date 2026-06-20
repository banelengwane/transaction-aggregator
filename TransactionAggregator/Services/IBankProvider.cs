using TransactionAggregator.Models;

namespace TransactionAggregator.Services
{
    public interface IBankProvider
    {
        string BankName { get; }
        Task<IEnumerable<Transaction>> FetchTransactionsAsync();
    }
}
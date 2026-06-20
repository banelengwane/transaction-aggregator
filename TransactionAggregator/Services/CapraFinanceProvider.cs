// File: Services/CapraFinanceProvider.cs
using TransactionAggregator.Models;

namespace TransactionAggregator.Services
{
    public class CapraFinanceProvider : IBankProvider
    {
        public string BankName => "CapraFinance";

        public async Task<IEnumerable<Transaction>> FetchTransactionsAsync()
        {
            // Simulate a slightly slower external response
            await Task.Delay(250); 

            return new List<Transaction>
            {
                new() { Id = "CPR-1102", SourceBank = BankName, Amount = -89.99m, Category = "Entertainment", TransactionDate = DateTime.UtcNow.AddDays(-2), Description = "Streaming Subscription" },
                new() { Id = "CPR-1103", SourceBank = BankName, Amount = -650.00m, Category = "Groceries", TransactionDate = DateTime.UtcNow.AddDays(-4), Description = "Woolworths Food" },
                new() { Id = "CPR-1104", SourceBank = BankName, Amount = -300.00m, Category = "Transport", TransactionDate = DateTime.UtcNow.AddDays(-12), Description = "Uber Rides" }
            };
        }
    }
}
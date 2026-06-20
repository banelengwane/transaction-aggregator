// File: Services/ApexBankProvider.cs
using TransactionAggregator.Models;

namespace TransactionAggregator.Services
{
    public class ApexBankProvider : IBankProvider
    {
        public string BankName => "ApexBank";

        public async Task<IEnumerable<Transaction>> FetchTransactionsAsync()
        {
            // Simulate network latency of hitting a real bank API
            await Task.Delay(120); 
            
            return new List<Transaction>
            {
                new() { Id = "APX-9081", SourceBank = BankName, Amount = -450.00m, Category = "Groceries", TransactionDate = DateTime.UtcNow.AddDays(-1), Description = "Pick n Pay Spend" },
                new() { Id = "APX-9082", SourceBank = BankName, Amount = 24500.00m, Category = "Salary", TransactionDate = DateTime.UtcNow.AddDays(-5), Description = "Monthly Salary Deposit" },
                new() { Id = "APX-9083", SourceBank = BankName, Amount = -120.00m, Category = "Transport", TransactionDate = DateTime.UtcNow.AddDays(-10), Description = "Fuel Station" }
            };
        }
    }
}
using Moq;
using TransactionAggregator.Models;
using TransactionAggregator.Services;

namespace TransactionAggregator.Tests;

public class AggregatorServiceTests
{
    private readonly Mock<IBankProvider> _mockBankA;
    private readonly Mock<IBankProvider> _mockBankB;

    public AggregatorServiceTests()
    {
        // initialize fresh mocks for every test run 
        _mockBankA = new Mock<IBankProvider>();
        _mockBankB = new Mock<IBankProvider>();

        _mockBankA.Setup(b => b.BankName).Returns("BankA");
        _mockBankA.Setup(b => b.BankName).Returns("BankB");
    }

    [Fact]
    public async Task GetTransactionsAsync_ShouldAggregateAndSortByLatestDate()
    {
        // arrange
        var bankATransactions = new List<Transaction>
        {
            new() { Id = "A1", SourceBank = "BankA", Amount=-100m, Category = "Groceries", TransactionDate = DateTime.UtcNow.AddDays(-2) }
        };
        var bankBTransactions = new List<Transaction>
        {
            new() { Id = "B1", SourceBank = "BankB", Amount=-200m, Category = "Transport", TransactionDate = DateTime.UtcNow.AddDays(-1) }
        };

        _mockBankA.Setup(b => b.FetchTransactionsAsync()).ReturnsAsync(bankATransactions);
        _mockBankB.Setup(b => b.FetchTransactionsAsync()).ReturnsAsync(bankBTransactions);

        var providers = new List<IBankProvider> { _mockBankA.Object, _mockBankB.Object};
        var service = new AggregatorService(providers);

        // Act 
        var (data, totalCount) = await service.GetTransactionsAsync(null, null, null, page: 1, pageSize: 10);

        Assert.Equal(2, totalCount);

        // the result list should order by latest date first (Bank B is 1 day ago, Bank A is 2 days ago)
        var resultList = data.ToList();
        Assert.Equal("B1", resultList[0].Id);
        Assert.Equal("A1", resultList[1].Id);
    }

    [Fact]
    public async Task GetTransactionsAsync_ShouldFilterByCategoryCorrectly()
    {
        // arrange
        var allTransactions = new List<Transaction>
        {
            new() { Id = "1", Category = "Groceries", TransactionDate = DateTime.UtcNow },
            new() { Id = "2", Category = "Salary", TransactionDate = DateTime.UtcNow },
        };

        _mockBankA.Setup(b => b.FetchTransactionsAsync()).ReturnsAsync(allTransactions);
        _mockBankB.Setup(b => b.FetchTransactionsAsync()).ReturnsAsync(new List<Transaction>()); // Empty

        var providers = new List<IBankProvider> { _mockBankA.Object, _mockBankB.Object };
        var service = new AggregatorService(providers);

        // act 
        var (data, totalCount) = await service.GetTransactionsAsync(null, null, "groceries", page: 1, pageSize: 10);

        // assert
        Assert.Equal(1, totalCount);
        Assert.Equal("Groceries", data.First().Category);
    }

    [Fact]
    public async Task GetTransactionsAsync_ShouldPaginateCorrectly()
    {
        // arrange 
        var largeDataset = new List<Transaction>();
        for (int i = 1; i <= 5; i++)
        {
            largeDataset.Add(new Transaction { Id = i.ToString(), TransactionDate = DateTime.UtcNow.AddHours(-1) });
        }

        _mockBankA.Setup(b => b.FetchTransactionsAsync()).ReturnsAsync(largeDataset);
        var providers = new List<IBankProvider> { _mockBankA.Object };
        var service = new AggregatorService(providers);

        // act: ask for page 2, with page size of 2 items
        var (data, totalCount) = await service.GetTransactionsAsync(null, null, null, page: 2, pageSize: 2);

        Assert.Equal(5, totalCount); // total global dataset size
        Assert.Equal(2, data.Count()); // items on this specific page
    }
}

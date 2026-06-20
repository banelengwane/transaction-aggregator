using TransactionAggregator.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddScoped<IBankProvider, ApexBankProvider>();
builder.Services.AddScoped<IBankProvider, CapraFinanceProvider>();

builder.Services.AddScoped<IAggregatorService, AggregatorService>();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

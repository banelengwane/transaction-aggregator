using TransactionAggregator.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
  options.AddPolicy("AllowFrontend", policy =>
  {
    policy.WithOrigins("http:localhost:5173")
      .AllowAnyHeader()
      .AllowAnyMethod();
  });
});

builder.Services.AddControllers();

builder.Services.AddScoped<IBankProvider, ApexBankProvider>();
builder.Services.AddScoped<IBankProvider, CapraFinanceProvider>();

builder.Services.AddScoped<IAggregatorService, AggregatorService>();

var app = builder.Build();

app.UseCors("AllowFrontend");

app.UseHttpsRedirection();
app.MapControllers();

app.Run();

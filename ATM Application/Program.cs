
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using ATM_Application.Services;
using ATM_Application.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IBankingService, BankingService>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddDbContext<BankingDbContext>(options =>
    options.UseSqlite("Data Source=banking.db"));

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();


app.MapControllers(); 

app.Run();

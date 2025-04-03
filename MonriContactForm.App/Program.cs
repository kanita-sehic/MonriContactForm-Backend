using AspNetCoreRateLimit;
using Microsoft.Data.SqlClient;
using MonriContactForm.App.Extensions;
using MonriContactForm.Core.Configuration;
using MonriContactForm.Core.Interfaces;
using MonriContactForm.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<AppSettings>(builder.Configuration);
builder.Services.ConfigureRateLimiting(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.RegisterDatabaseConnectionFactory(builder.Configuration);
builder.Services.RegisterRepositories();
builder.Services.RegisterServices();
builder.Services.RegisterHttpClients(builder.Configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    try
    {
        await InitializeDatabase(scope.ServiceProvider);
    }
    catch (Exception e)
    {
        // log the exception
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseIpRateLimiting();

app.UseAuthorization();

app.MapControllers();

app.Run();

async Task InitializeDatabase(IServiceProvider services)
{
    var masterConnectionString = builder.Configuration.GetConnectionString("SqlConnection")
        .Replace("Database=ContactForm", "Database=master");

    var createDatabaseScript = SqlScriptLoader.LoadScript("Migrations", "CreateDatabase");

    await using (var masterConnection = new SqlConnection(masterConnectionString))
    {
        await masterConnection.OpenAsync();
        var createDatabaseCommand = new SqlCommand(createDatabaseScript, masterConnection);
        await createDatabaseCommand.ExecuteNonQueryAsync();
    }

    var connectionFactory = services.GetRequiredService<IDatabaseConnectionFactory>();
    var createTablesScript = SqlScriptLoader.LoadScript("Migrations", "CreateTables");

    await using var connection = await connectionFactory.CreateConnectionAsync();
    await new SqlCommand(createTablesScript, connection).ExecuteNonQueryAsync();
}

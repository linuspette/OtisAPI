using Azure.Identity;
using Microsoft.EntityFrameworkCore;
using OtisAPI.DataAccess;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Configuration.AddAzureKeyVault(new Uri(builder.Configuration["Keyvault"]), new DefaultAzureCredential());

builder.Services.AddDbContext<SqlContext>(x =>
{
    x.UseSqlServer(builder.Configuration["OtisDbConnectionString"]);
});
builder.Services.AddDbContext<NoSqlContext>(x =>
    x.UseCosmos(
        builder.Configuration["OtisCosmosDbUri"],
        builder.Configuration["OtisCosmosDbAccessKey"],
        "LpSmartDevices"));


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

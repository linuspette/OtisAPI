using Azure.Identity;
using Microsoft.EntityFrameworkCore;
using OtisAPI.DataAccess;
using OtisAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Configuration.AddAzureKeyVault(new Uri(builder.Configuration["Keyvault"]), new DefaultAzureCredential());

builder.Services.AddDbContext<SqlContext>(x =>
{
    x.UseSqlServer(builder.Configuration["PC"]);
});
builder.Services.AddDbContext<NoSqlContext>(x =>
    x.UseCosmos(
        builder.Configuration["OtisCosmosDbUri"],
        builder.Configuration["OtisCosmosDbAccessKey"],
        "LpSmartDevices"));


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IElevatorService, ElevatorService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

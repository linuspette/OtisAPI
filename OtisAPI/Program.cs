using Azure.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OtisAPI.DataAccess;
using OtisAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Configuration.AddAzureKeyVault(new Uri(builder.Configuration["Keyvault"]), new DefaultAzureCredential());

builder.Services.AddDbContext<SqlContext>(x =>
{
    //x.UseSqlServer(builder.Configuration["PC"]);
    x.UseSqlServer(builder.Configuration["OtisDbConnectionString"]);
});
builder.Services.AddDbContext<NoSqlContext>(x =>
    x.UseCosmos(
        builder.Configuration["OtisCosmosDbUri"],
        builder.Configuration["OtisCosmosDbAccessKey"],
        "LpSmartDevices"));


builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IElevatorService, ElevatorService>();
builder.Services.AddScoped<IErrandService, ErrandService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<ICosmosDbService, CosmosDbService>();

var app = builder.Build();

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin  
    .AllowCredentials());               // allow credentials 


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

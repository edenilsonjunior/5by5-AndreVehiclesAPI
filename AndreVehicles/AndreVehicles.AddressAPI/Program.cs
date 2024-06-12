using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AndreVehicles.AddressAPI.Data;
using Microsoft.Extensions.Options;
using Services.Financials;
using Services.Utils;
using Services.People;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AndreVehiclesAddressAPIContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AndreVehiclesAddressAPIContext") ?? throw new InvalidOperationException("Connection string 'AndreVehiclesAddressAPIContext' not found.")));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.Configure<MongoDataBaseSettings>(
               builder.Configuration.GetSection(nameof(MongoDataBaseSettings)));

builder.Services.AddSingleton<IMongoDataBaseSettings>(sp =>
    sp.GetRequiredService<IOptions<MongoDataBaseSettings>>().Value);

builder.Services.AddSingleton<AddressService>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

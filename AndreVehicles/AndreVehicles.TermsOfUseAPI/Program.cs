using Microsoft.Extensions.Options;
using Services.Financials;
using Services.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.Configure<MongoDataBaseSettings>(
               builder.Configuration.GetSection(nameof(MongoDataBaseSettings)));

builder.Services.AddSingleton<IMongoDataBaseSettings>(sp =>
    sp.GetRequiredService<IOptions<MongoDataBaseSettings>>().Value);

builder.Services.AddSingleton<TermsOfUseService>();
builder.Services.AddSingleton<TermsOfUseAcceptService>();


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

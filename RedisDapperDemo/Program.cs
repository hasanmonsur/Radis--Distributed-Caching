using RedisDapperDemo.Contacts;
using RedisDapperDemo.Data;
using RedisDapperDemo.Services;
using StackExchange.Redis;
using System.Data;

var builder = WebApplication.CreateBuilder(args);


// Configure Redis
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(builder.Configuration["Redis:Configuration"]));

builder.Services.AddControllers();
// Configure Dapper
builder.Services.AddTransient<DapperContext>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();


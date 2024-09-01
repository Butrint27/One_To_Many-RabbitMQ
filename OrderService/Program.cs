using AutoMapper;
using Messaging.Shared.Extensions;
using OrderService.Data;
using OrderService.Mapper;
using OrderService.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<OrderDbContext>(provider =>
    new OrderDbContext(builder.Configuration.GetConnectionString("MongoDb")));

builder.Services.AddScoped<IOrderServices, OrderServices>();

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddMessagingServices(); // Add MassTransit and RabbitMQ

builder.Services.AddHttpClient();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

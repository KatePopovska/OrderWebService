using System.Reflection;

using Microsoft.OpenApi.Models;

using OrderWebService.Domain;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Order Web Service API",
        Description = "Order Web Service API",
    });
});

builder.Services.AddDomainServices(builder.Configuration.GetConnectionString("AppDbConnection"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.MapControllers();

app.Run();

public partial class Program { }
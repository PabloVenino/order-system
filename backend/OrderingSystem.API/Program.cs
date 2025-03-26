using OrderingSystem.API.Endpoints;
using OrderingSystem.Application;
using OrderingSystem.Infra;
using OrderingSystem.Infra.Middlewares;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
  .AddApplication()
  .AddInfra(builder.Configuration);

// Adding JSON Serializable Responses
builder.Services.ConfigureHttpJsonOptions(opt => {
  opt.SerializerOptions.PropertyNamingPolicy = null; // for PascalCase
  opt.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

// Adding Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt => {
  opt.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo{
    Title = "Ordering System for TMB",
    Version = "v1.0.0",
    Description = "API for management of Orders (PoC)"
  });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI(opt => {
    opt.SwaggerEndpoint("/swagger/v1/swagger.json", "Ordering System for TMB");
    opt.RoutePrefix = "swagger";
  });
}

app.UseHttpsRedirection();
app.UseMiddleware<RateLimitingMiddleware>();
// Map Minimal API Controllers
app.RegisterOrderSystemEndpoints();

// Runs
app.Run();
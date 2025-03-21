using OrderingSystem.API.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


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
  app.MapOpenApi();
}

app.UseHttpsRedirection();

// Map Minimal API Controllers
app.RegisterOrderSystemEndpoints();

// Runs
app.Run();
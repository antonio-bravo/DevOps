using Swashbuckle.AspNetCore.Swagger;
using System.Reflection;
using GloboTicket.Catalog.Repositories;
using Microsoft.OpenApi.Models;
// Add this line to your existing using statements
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

builder.Services.AddTransient<IEventRepository, AzureStorageEventRepository>();
//builder.Services.AddTransient<IEventRepository, InMemoryEventRepository>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Register the Swagger generator
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "GloboTicket API", Version = "v1" });
    // Set the comments path for the Swagger JSON and UI.
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});    
});

var app = builder.Build();

// Enable middleware to serve generated Swagger as a JSON endpoint
app.UseSwagger();

// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
// specifying the Swagger JSON endpoint
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "GloboTicket API V1");
    c.RoutePrefix = string.Empty;  // to serve the Swagger UI at the app's root    

});

app.UseAuthorization();

app.MapControllers();
app.Run();
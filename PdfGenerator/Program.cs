using Microsoft.OpenApi.Models;
using PdfGenerator.Options;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services
  .AddOptions<PdfOptions>()
  .Bind(builder.Configuration.GetSection(PdfOptions._SectionKey))
  .ValidateDataAnnotations();

builder.Services.AddMvc();

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });

builder.Services.AddCors();

builder.Services.AddSwaggerGen(o =>
{
    o.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Pdf Generator",
        Version = "v1",
        Description = "Pdf Generator REST API"
    });
});

var app = builder.Build();

app.UseRouting();
app.UseCors(o => o
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowAnyOrigin());

app.UseSwagger(o => o.RouteTemplate = "/docs/{documentName}/swagger.json");

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();

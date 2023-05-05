using Innowise.Clinic.Services.Configuration;
using Innowise.Clinic.Services.Persistence;
using Innowise.Clinic.Services.RequestPipeline;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<ServicesDbContext>(opts =>
    opts.UseSqlServer(builder.Configuration.GetConnectionString("default")));

builder.Services.ConfigureSwagger();
builder.Services.ConfigureSecurity();
builder.Services.RegisterServices(builder.Configuration);
builder.Services.ConfigureCrossServiceCommunication(builder.Configuration);
builder.ConfigureSerilog();

var app = builder.Build();

await app.PrepareDatabase();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

Log.Information("The Services service is starting");
app.Run();
Log.Information("The Services service is stopping");
await Log.CloseAndFlushAsync();
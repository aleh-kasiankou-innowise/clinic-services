using Innowise.Clinic.Services.Configuration;
using Innowise.Clinic.Services.Persistence;
using Innowise.Clinic.Services.RequestPipeline;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<ServicesDbContext>(opts =>
    opts.UseSqlServer(builder.Configuration.GetConnectionString("default")));

builder.Services.ConfigureSwagger();
builder.Services.ConfigureSecurity();
builder.Services.RegisterServices();

var app = builder.Build();

await app.PrepareDatabase();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
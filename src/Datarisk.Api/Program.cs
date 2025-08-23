using Datarisk.Api.Controllers;
using Datarisk.Application.Commands;
using Datarisk.Application.Queries;
using Datarisk.Application.Services;
using Datarisk.Core.Interfaces;
using Datarisk.Infrastructure.Data;
using Datarisk.Infrastructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
builder.Services.AddDbContext<DatariskDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositories
builder.Services.AddScoped<IScriptRepository, ScriptRepository>();
builder.Services.AddScoped<IProcessingRepository, ProcessingRepository>();
builder.Services.AddScoped<IScriptExecutionRepository, ScriptExecutionRepository>();

// Services
builder.Services.AddScoped<IScriptExecutionService, ScriptExecutionService>();

// MediatR
builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(typeof(CreateScriptCommand).Assembly);
});

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

// Ensure database is created
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DatariskDbContext>();
    context.Database.EnsureCreated();
}

app.Run();

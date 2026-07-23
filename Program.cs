using Microsoft.EntityFrameworkCore;
using TesteDevjr.Infrastructure.Data;
using TesteDevjr.Repositories;
using TesteDevjr.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<TaskManagementDbContext>(options =>
    options.UseInMemoryDatabase("TaskManagementDb"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using TaskManager.Application.Interfaces;
using TaskManager.Application.Services;
using TaskManager.Domain.Entities.TaskItems;
using TaskManager.Infrastructure.Data.SqlServer.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TaskDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("TaskManagerConnectionString")));
builder.Services.AddScoped<ITaskService, TaskService>();

var app = builder.Build();


app.Run(async (HttpContext context) =>
{
    var taskService = context.RequestServices.GetRequiredService<ITaskService>();

    var path = context.Request.Path.Value?.ToLower();
    var method = context.Request.Method.ToUpper();

    if (method == "GET" && path == "/tasks")
    {
        var tasks = await taskService.GetAllAsync();
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(JsonSerializer.Serialize(tasks));
    }
    else if (method == "POST" && path == "/tasks")
    {
        using var reader = new StreamReader(context.Request.Body);
        var body = await reader.ReadToEndAsync();

        var requestedTask = JsonSerializer.Deserialize<TaskItem>(body);

        var task = await taskService.CreateAsync(requestedTask.Title, requestedTask.Description);
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = 201;
        await context.Response.WriteAsync(JsonSerializer.Serialize(task));
    }
    else if (method == "POST" && path == "/tasks/done")
    {
        using var reader = new StreamReader(context.Request.Body);
        var body = await reader.ReadToEndAsync();
        var id = JsonSerializer.Deserialize<Dictionary<string, Guid>>(body).GetValueOrDefault("id");

        var result = await taskService.MarkAsDoneAsync(id);
        context.Response.StatusCode = result ? 200 : 500;
    }
    else if (method == "POST" && path == "/tasks/cancel")
    {
        using var reader = new StreamReader(context.Request.Body);
        var body = await reader.ReadToEndAsync();
        var id = JsonSerializer.Deserialize<Dictionary<string, Guid>>(body).GetValueOrDefault("id");

        var result = await taskService.CancelAsync(id);
        context.Response.StatusCode = result ? 200 : 500;
    }
    else
    {
        context.Response.StatusCode = 404;
        await context.Response.WriteAsync("Not Found");
    }
});

app.Run();

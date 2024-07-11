using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Rewrite;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<ITaskService>(new InMemoryTaskService());

var app = builder.Build();

app.UseRewriter(new RewriteOptions().AddRedirect("task/(.*)", "todos/$1"));

var todos = new List<Todo>();

app.MapGet("/", () => "Hello World!");

app.MapGet("/todos", (ITaskService Service) => Service.GetTodos());

app.MapGet("/todos/{id}", Results<Ok<Todo>, NotFound> (int id, ITaskService Service) =>
{
    var targetTodo = Service.GetToDoById(id);
    return targetTodo is null ? TypedResults.NotFound() : TypedResults.Ok(targetTodo);
});

app.MapPost("/todos", (Todo task, ITaskService Service) =>
{
    Service.AddToDo(task);
    return TypedResults.Created("/todos/{id}", task);
}).AddEndpointFilter(async (context, next) =>
{
    var taskArgument = context.GetArgument<Todo>(0);
    var error = new Dictionary<string, string[]>();

    if (taskArgument.DueDate < DateTime.UtcNow)
    {
        error.Add(nameof(Todo.DueDate), ["Cannot have the due date in the past"]);
    }

    if (taskArgument.IsCompleted)
    {
        error.Add(nameof(Todo.IsCompleted), ["Cannot add completed todo"]);
    }

    if (error.Count > 0)
    {
        return Results.ValidationProblem(error);
    }

    return await next(context);
});

app.MapDelete("/todos/{id}", (int id, ITaskService Service) =>
{
    Service.DeleteToDoById(id);
    return TypedResults.NoContent();
});

app.Run();

public record Todo(int Id, string Name, DateTime DueDate, bool IsCompleted);

interface ITaskService
{
    Todo? GetToDoById(int id);
    List<Todo> GetTodos();
    void DeleteToDoById(int id);
    Todo AddToDo(Todo task);
}

class InMemoryTaskService : ITaskService
{
    private readonly List<Todo> _todos = [];
    public Todo AddToDo(Todo task)
    {
        _todos.Add(task);
        return task;
    }

    public void DeleteToDoById(int id)
    {
        _todos.RemoveAll(t => t.Id == id);
    }

    public Todo? GetToDoById(int id)
    {
        return _todos.SingleOrDefault(t => t.Id == id);
    }

    public List<Todo> GetTodos()
    {
        return _todos;
    }
}
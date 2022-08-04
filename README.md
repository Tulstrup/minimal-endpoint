MinimalEndpoint
=======

NuGet package for registering ASP.NET Core endpoints in a minimal fashion.

### Installing MinimalEndpoint

You should install [MinimalEndpoint with NuGet](https://www.nuget.org/packages/MinimalEndpoint):

    Install-Package MinimalEndpoint

Or via the .NET Core command line interface:

    dotnet add package MinimalEndpoint

### Using MinimalEndpoint

First, register all minimal endpoints in your `Program.cs` before `app.Run()`.

```csharp
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapMinimalEndpoints(Assembly.GetExecutingAssembly());

app.Run();
```

Next, add any of the `MinimalEndpoint` attributes to a static method. 
This will make this method handle request to the given url pattern.

The following attributes are supported: `MinimalGet`, `MinimalPost`, `MinimalPut`, `MinimalPatch`, `MinimalDelete`.

```csharp
public static class CreateUserEndpoint
{
    public record Request(string Name, DateTimeOffset Birthdate);
    public record Response(Guid CreatedUserId);
    
    [MinimalPost("/api/v1/users")]
    public static async Task<Response> Handle(
        Request request,
        IUserService userService)
    {
        var createdUserId = new User(request.Name, request.Birthdate);
        
        await userService.Save(user);
            
        return new Response(createdUserId);
    }
}
```
Parameters to the `Handle` method will be automatically provided from the http request or service collection, like any regular [ASP.NET Core MinimalApi](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis?view=aspnetcore-6.0).

Ideally, create one file per endpoint containing both the request, response and handler method.
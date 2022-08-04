using System.Linq.Expressions;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace MinimalEndpoint;

public static class WebApplicationExtensions
{
    public static WebApplication MapMinimalEndpoints(
        this WebApplication app,
        Assembly assemblyContainingMinimalEndpoints)
    {
        assemblyContainingMinimalEndpoints.GetTypes()
            .SelectMany(t => t.GetMethods())
            .Where(m => m.GetCustomAttributes(typeof(MinimalEndpointAttribute), false).Length > 0)
            .ToList()
            .ForEach(x => MapEndpoint(app, x));

        return app;
    }

    private static void MapEndpoint(IEndpointRouteBuilder app, MethodInfo method)
    {
        var minimalEndpoint = method.GetCustomAttribute<MinimalEndpointAttribute>()!;
        var methodDelegate = method.CreateDelegate(Expression.GetDelegateType(
                method
                    .GetParameters()
                    .Select(x => x.ParameterType)
                    .Concat(new[] { method.ReturnType })
                    .ToArray()));

        var routeHandlerBuilder = minimalEndpoint.Method switch
        {
            Http.Get => app.MapGet(minimalEndpoint.Pattern, methodDelegate),
            Http.Post => app.MapPost(minimalEndpoint.Pattern, methodDelegate),
            Http.Put => app.MapPut(minimalEndpoint.Pattern, methodDelegate),
            Http.Delete => app.MapDelete(minimalEndpoint.Pattern, methodDelegate),
            Http.Patch => app.MapMethods(minimalEndpoint.Pattern, new[] { "patch" }, methodDelegate),
            _ => throw new NotImplementedException(
                $"{nameof(MinimalEndpointAttribute)} does not currently support http method {minimalEndpoint.Method}")
        };

        if (minimalEndpoint.AuthPolicies != null)
            routeHandlerBuilder.RequireAuthorization(minimalEndpoint.AuthPolicies);
    }
}
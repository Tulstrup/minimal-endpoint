namespace MinimalEndpoint;

public class MinimalEndpointAttribute : Attribute
{
    public Http Method { get; }
    public string Pattern { get; }
    public string[]? AuthPolicies { get; }

    public MinimalEndpointAttribute(Http method, string pattern, params string[] authPolicies)
    {
        Method = method;
        Pattern = pattern;
        AuthPolicies = authPolicies;
    }
}

public class MinimalGetAttribute : MinimalEndpointAttribute
{
    public MinimalGetAttribute(string pattern, params string[] authPolicies) : base(Http.Get, pattern, authPolicies) { }
}

public class MinimalPostAttribute : MinimalEndpointAttribute
{
    public MinimalPostAttribute(string pattern, params string[] authPolicies) : base(Http.Post, pattern, authPolicies) { }
}

public class MinimalPutAttribute : MinimalEndpointAttribute
{
    public MinimalPutAttribute(string pattern, params string[] authPolicies) : base(Http.Put, pattern, authPolicies) { }
}

public class MinimalDeleteAttribute : MinimalEndpointAttribute
{
    public MinimalDeleteAttribute(string pattern, params string[] authPolicies) : base(Http.Delete, pattern, authPolicies) { }
}

public class MinimalPatchAttribute : MinimalEndpointAttribute
{
    public MinimalPatchAttribute(string pattern, params string[] authPolicies) : base(Http.Patch, pattern, authPolicies) { }
}

using System.Net.Http;

namespace Blazor.HttpInterfaceProxy;

public abstract class ApiMethodAttribute : Attribute
{
    public string? Route { get; }
    protected ApiMethodAttribute(string? route=null) => Route = route;
    public abstract HttpMethod Method { get; }
}

public class ApiGetAttribute : ApiMethodAttribute
{
    public ApiGetAttribute(string? r=null):base(r){}
    public override HttpMethod Method => HttpMethod.Get;
}

public class ApiPostAttribute : ApiMethodAttribute
{
    public ApiPostAttribute(string? r=null):base(r){}
    public override HttpMethod Method => HttpMethod.Post;
}

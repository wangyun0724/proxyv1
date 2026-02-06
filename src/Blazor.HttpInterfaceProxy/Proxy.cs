
using System.Reflection;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;

namespace Blazor.HttpInterfaceProxy;

public class ProxyOptions
{
    public string Prefix = "/api/web";
    public JsonSerializerOptions Json = new(){PropertyNameCaseInsensitive=true};
}

class Dispatch<T> : DispatchProxy
{
    public HttpClient Client = default!;
    public ProxyOptions Opt = default!;

    protected override object Invoke(MethodInfo m, object[] a)
        => Call(m,a);

    async Task<object?> Call(MethodInfo m, object[] a)
    {
        var api = m.GetCustomAttribute<ApiMethodAttribute>()!;
        var svc = typeof(T).Name.Replace("I","").Replace("Service","");
        var url = $"{Opt.Prefix}/{svc}/{(api.Route??m.Name)}";

        var req = new HttpRequestMessage(api.Method,url);
        if (api.Method!=HttpMethod.Get && a.Length>0)
            req.Content = System.Net.Http.Json.JsonContent.Create(a[0]);

        var resp = await Client.SendAsync(req);
        var txt = await resp.Content.ReadAsStringAsync();
        resp.EnsureSuccessStatusCode();

        var rt = m.ReturnType.GenericTypeArguments.FirstOrDefault();
        if (rt==null) return null;
        return JsonSerializer.Deserialize(txt, rt, Opt.Json);
    }
}

public static class ProxyExtensions
{
    public static IServiceCollection AddHttpInterfaceProxies(
        this IServiceCollection s, Assembly asm)
    {
        s.AddSingleton(new ProxyOptions());
        var itfs = asm.GetTypes().Where(t=>t.IsInterface && t.Name.EndsWith("Service"));

        foreach(var itf in itfs)
        {
            s.AddScoped(itf, sp=>{
                var c = sp.GetRequiredService<HttpClient>();
                var o = sp.GetRequiredService<ProxyOptions>();
                var p = DispatchProxy.Create(itf, typeof(Dispatch<>).MakeGenericType(itf));
                dynamic d = p!;
                d.Client=c; d.Opt=o;
                return p!;
            });
        }
        return s;
    }
}

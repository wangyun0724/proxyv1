
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazor.HttpInterfaceProxy;
using Sample.BlazorWasm;

var b = WebAssemblyHostBuilder.CreateDefault(args);
b.RootComponents.Add<App>("#app");

b.Services.AddScoped(_=> new HttpClient{BaseAddress=new Uri("https://localhost:5001")});
b.Services.AddHttpInterfaceProxies(typeof(ITestService).Assembly);

await b.Build().RunAsync();

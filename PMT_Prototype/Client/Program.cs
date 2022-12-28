using Blazored.Toast;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using PMT_Prototype.Client;
using PMT_Prototype.Client.Clients;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient<InitialStartupClient>(client =>
{
    client.BaseAddress = new Uri($"{builder.HostEnvironment.BaseAddress}InitialStartup/");
});

builder.Services.AddHttpClient<WeatherClient>(client =>
{
    client.BaseAddress = new Uri($"https://api.weather.gov/alerts/");
});

builder.Services.AddBlazoredToast();

await builder.Build().RunAsync();

global using Microsoft.AspNetCore.Components.Authorization; 
using Frontend.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Syncfusion.Blazor;


namespace Frontend
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(builder.Configuration["Syncfusion"]);
            builder.Services.AddSyncfusionBlazor();
            builder.Services.AddSyncfusionBlazor(options => { options.IgnoreScriptIsolation = false; });


            builder.Services.AddScoped(x =>
            {
                var apiUrl = new Uri(builder.Configuration["apiUrl"]);

                return new HttpClient() { BaseAddress = apiUrl };
            });

            builder.Services
                            .AddScoped<AuthenticationStateProvider, AuthenticationService>()
                            .AddScoped<ILocalStorageService, LocalStorageService>();

            builder.Services.AddAuthorizationCore();

            await builder.Build().RunAsync();
        }
    }
}




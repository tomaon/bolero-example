namespace App.Server

open Microsoft.AspNetCore.Authentication.Cookies
open Microsoft.AspNetCore.Builder
open Microsoft.EntityFrameworkCore
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection

open Bolero.Remoting.Server


type Startup(configuration: IConfiguration) =

    let configuration = configuration

    member __.ConfigureServices(serviceCollection: IServiceCollection) =
        serviceCollection

            .AddDbContext<AppDbContext>(fun dbContextOptionsBuilder ->
                dbContextOptionsBuilder
                    .UseSqlite(configuration.GetConnectionString("Sqlite"))
                |> ignore
            )
#if !DEBUG
            .AddResponseCompression()
#endif
            .AddAuthorization()
            .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie()
            .Services

            .AddMvcCore()
            .Services

            .AddRemoting<Services.V1Service>()

        |> ignore

    member __.Configure(applicationBuilder: IApplicationBuilder) =
        applicationBuilder
            .UseDefaultFiles()
            .UseStaticFiles()
            .UseClientSideBlazorFiles<App.Client.Startup>()
#if !DEBUG
            .UseResponseCompression()
#endif
            .UseCookiePolicy()
            .UseRouting()
            .UseAuthentication()
            // .UseAuthorization()
            .UseRemoting()
            .UseEndpoints(fun endpointRouteBuilder ->
                endpointRouteBuilder
                    .MapFallbackToClientSideBlazor<App.Client.Startup>("index.html")
                |> ignore
            )
        |> ignore

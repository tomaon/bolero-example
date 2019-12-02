namespace App.Client

open Microsoft.AspNetCore.Components.Builder
open Microsoft.Extensions.DependencyInjection

open Bolero.Remoting.Client


type Startup() =

    member __.ConfigureServices(serviceCollection: IServiceCollection) =
        serviceCollection
            .AddRemoting()
        |> ignore

    member __.Configure(componentsApplicationBuilder: IComponentsApplicationBuilder) =
        componentsApplicationBuilder
            .AddComponent<Index.Main>("#main")

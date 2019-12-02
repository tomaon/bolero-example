namespace App.Client

open Microsoft.AspNetCore.Blazor.Hosting


module Program =

    [<EntryPoint>]
    let main _ =
        BlazorWebAssemblyHost
            .CreateDefaultBuilder() // TODO
            .UseBlazorStartup<Startup>()
            .Build()
            .Run()
        0

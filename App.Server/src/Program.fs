namespace App.Server

open System.IO

open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging


module Program =

    [<EntryPoint>]
    let main args =
        HostBuilder()
            .ConfigureWebHost(fun webHostBuilder ->
                webHostBuilder
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .UseStartup<Startup>()
                    .UseKestrel()
                    .UseIISIntegration()
                |> ignore
            )
            .ConfigureAppConfiguration(fun hostBuilderContext configurationBuilder ->
                let hostEnvironment = hostBuilderContext.HostingEnvironment
                configurationBuilder
                    .AddJsonFile(sprintf "appsettings.%s.json" hostEnvironment.EnvironmentName)
                    .AddCommandLine(args)
                |> ignore
            )
            .ConfigureLogging(fun hostBuilderContext loggingBuilder ->
                let configuration = hostBuilderContext.Configuration
                loggingBuilder
                    .AddConfiguration(configuration.GetSection("Logging"))
                    .AddConsole()
                |> ignore
            )
            .Build()
            .Run()
        0

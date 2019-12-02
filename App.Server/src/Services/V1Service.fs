namespace App.Server.Services

open System

open Bolero.Remoting.Server

open App.Client.Services
open App.Server
open App.Server.Extensions


type V1Service(remoteContext: IRemoteContext) =
    inherit RemoteHandler<App.Client.Services.V1Service>()

    let getBooks () =
        async {
            use dbContext = remoteContext.GetDbContext<AppDbContext>()
            return [| for e in Book.FindAllAsNoTracking dbContext -> e |]
        }

    let getUsername () =
        async {
            return remoteContext.HttpContext.User.Identity.Name
        }

    let signIn (username, password) =
        async {
            if password = "password" then
                do! remoteContext.HttpContext.AsyncSignIn(username, TimeSpan.FromDays(365.))
                return Ok username
            else
                return Error "Sign in failed. Use any username and the password \"password\"."
        }

    let signOut () =
        async {
            return! remoteContext.HttpContext.AsyncSignOut()
        }

    override __.Handler =
        { GetBooks = remoteContext.Authorize getBooks
          GetUsername = remoteContext.Authorize getUsername
          SignIn = signIn
          SignOut = signOut }

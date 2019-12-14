namespace App.Client

open System

open Bolero.Json
open Bolero.Remoting


module Services =


    [<CLIMutable>]
    type Book =
        { Id: int
          Title: string
          Author: string
          [<DateTimeFormat "yyyy-MM-dd">]
          PublishDate: DateTime
          Isbn: string }


    type V1Service =
        {
            GetBooks: unit -> Async<Result<Book[], string>>
            GetUsername: unit -> Async<Result<string, string>>
            SignIn: string * string -> Async<Result<string, string>>
            SignOut: unit -> Async<unit>
        }

        interface IRemoteService with
            member __.BasePath = "/v1"

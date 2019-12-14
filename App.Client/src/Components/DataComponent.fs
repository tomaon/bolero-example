namespace App.Client.Components

open Elmish
open Bolero
open Bolero.Html
open Bolero.Remoting

open App.Client.Extensions
open App.Client.Services


module DataComponent =


    [<RequireQualifiedAccess>]
    type Msg =
        | RecvGetBooks of Result<Book[], string>
        | SendGetBooks
        | RecvError of exn
        | Error of string


    type Model =
        { Books: Book[] option }


    let init =
        { Books = None }, Cmd.ofMsg (Msg.SendGetBooks)


    let update (remoteProvider: IRemoteProvider) (msg: Msg) (model: Model) =
        match msg with

        | Msg.RecvGetBooks result ->
            match result with
            | Ok books ->
                { model with Books = Some books }, Cmd.none
            | Error message ->
                model, Cmd.ofMsg (Msg.Error message)
        | Msg.SendGetBooks ->
            model, Cmd.ofAsync remoteProvider.V1.GetBooks () Msg.RecvGetBooks Msg.RecvError

        | Msg.RecvError _ | Msg.Error _ ->
            model, Cmd.none


    let view (router: Router<_, _, _>) (model: Model) (dispatch: Dispatch<Msg>) =
        section [ "id" => "data"; "class" => "section" ] [
            cond model.Books <| function
                | Some value when value.Length > 0 ->
                    table [ "class" => "table is-fullwidth" ] [
                        thead [] [
                            tr [] [
                                td [] [ text "Title" ]
                                td [] [ text "Author" ]
                                td [] [ text "Published" ]
                                td [] [ text "ISBN" ]
                            ]
                        ]
                        tbody [] [
                            forEach value <| fun book ->
                                tr [] [
                                    td [] [ text book.Title ]
                                    td [] [ text book.Author ]
                                    td [] [ text (book.PublishDate.ToString("yyyy-MM-dd")) ]
                                    td [] [ text book.Isbn ]
                                ]
                        ]
                    ]
                | Some value ->
                    text "no data"
                | None ->
                    text "Loading..." // TODO
        ]

namespace App.Client.Components

open Elmish
open Bolero
open Bolero.Html
open Bolero.Remoting

open App.Client.Services


module DataComponent =


    type Msg = unit


    type Model =
        { Books: Book[] }


    let init (books: Book[]) =
        { Books = books }, Cmd.none


    let update (remoteProvider: IRemoteProvider) (msg: Msg) (model: Model) =
        model, Cmd.none


    let view (router: Router<_, _, _>) (model: Model) (dispatch: Dispatch<Msg>) =
        section [ "id" => "data"; "class" => "section" ] [
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
                    forEach model.Books <| fun book ->
                        tr [] [
                            td [] [ text book.Title ]
                            td [] [ text book.Author ]
                            td [] [ text (book.PublishDate.ToString("yyyy-MM-dd")) ]
                            td [] [ text book.Isbn ]
                        ]
                ]
            ]
        ]

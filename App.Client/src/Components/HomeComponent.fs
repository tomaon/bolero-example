namespace App.Client.Components

open Elmish
open Bolero
open Bolero.Html
open Bolero.Remoting

open App.Client


module HomeComponent =


    type Msg = unit


    type Model = unit


    let init =
        (), Cmd.none


    let update (remoteProvider: IRemoteProvider) (msg: Msg) (model: Model) =
        model, Cmd.none


    let view (router: Router<_, _, _>) (model: Model) (dispatch: Dispatch<Msg>) =
        section [ "id" => "home"; "class" => "section" ] [
            h1 [ "class" => "title" ] [ text "Welcome to Bolero!" ]
            div [ "class" => "content" ] [
                p [] [ text "This application demonstrates Bolero's major features." ]
                ul [] [
                    li [] [
                        text "The entire application is driven by "
                        a [ "target" => "_blank"; "href" => "https://fsbolero.github.io/docs/Elmish" ] [ text "Elmish" ]
                    ]
                    li [] [
                        text "The menu on the left switches pages based on "
                        a [ "target" => "_blank"; "href" => "https://fsbolero.github.io/docs/Routing" ] [ text "routes" ]
                    ]
                    li [] [
                        text "The "
                        a [ router.HRef Page.Counter ] [ text "Counter" ]
                        text " page demonstrates event handlers and data binding in "
                        a [ "target" => "_blank"; "href" => "https://fsbolero.github.io/docs/Templating" ] [ text "HTML templates" ]
                    ]
                    li [] [
                        text "The "
                        a [ router.HRef Page.Data ] [ text "Download data" ]
                        text " page demonstrates the use of "
                        a [ "target" => "_blank"; "href" => "https://fsbolero.github.io/docs/Remoting" ] [ text "remote functions" ]
                    ]
                ]
                p [] [ text "Enjoy writing awesome apps!" ]
            ]
        ]

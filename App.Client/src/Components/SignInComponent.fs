namespace App.Client.Components

open Elmish
open Bolero
open Bolero.Html
open Bolero.Remoting

open App.Client.Extensions


module SignInComponent =


    [<RequireQualifiedAccess>]
    type Msg =
        | SetUsername of string
        | SetPassword of string
        | RecvSignIn of Result<string, string>
        | SendSignIn
        | RecvError of exn
        | Ok of string
        | Error of string


    type Model =
        { Username: string
          Password: string
          Error: string option }


    let init () =
        { Username = ""
          Password = ""
          Error = None }, Cmd.none


    let update (remoteProvider: IRemoteProvider) (msg: Msg) (model: Model) =
        match msg with

        | Msg.SetUsername value ->
            { model with Username = value; Error = None }, Cmd.none
        | Msg.SetPassword value ->
            { model with Password = value; Error = None }, Cmd.none

        | Msg.RecvSignIn result ->
            match result with
            | Ok value ->
                model, Cmd.ofMsg (Msg.Ok value)
            | Error value ->
                { model with Error = Some value }, Cmd.none
        | Msg.SendSignIn ->
            { model with Error = None }, Cmd.ofAsync remoteProvider.V1.SignIn (model.Username, model.Password) Msg.RecvSignIn Msg.RecvError

        | Msg.RecvError e ->
            model, Cmd.ofMsg (Msg.Error e.Message)

        | Msg.Ok _ | Msg.Error _ ->
            model, Cmd.none


    let view (router: Router<_, _, _>) (model: Model) (dispatch: Dispatch<Msg>) =
        section [ "id" => "auth"; "class" => "section" ] [
            h1 [ "class" => "title" ] [ text "Sign in" ]
            form [ on.submit (fun _ -> dispatch (Msg.SendSignIn))] [
                div [ "class" => "field" ] [
                    label [ "class" => "label" ] [ text "Username:" ]
                    div [ "class" => "control" ] [
                        input [ "type" => "text"; "class" => "input"; bind.change model.Username (Msg.SetUsername >> dispatch) ]
                    ]
                ]
                div [ "class" => "field" ] [
                    label [ "class" => "label" ] [ text "Password:" ]
                    div [ "class" => "control" ] [
                        input [ "type" => "password"; "class" => "input"; bind.change model.Password (Msg.SetPassword >> dispatch) ]
                    ]
                ]
                div [ "class" => "field" ] [
                    div [ "class" => "control" ] [
                        input [ "type" => "submit"; "class" => "button"; "value" => "Sign in" ]
                    ]
                ]
                cond model.Error <| function
                    | Some value ->
                        div [ "class" => "notification is-warning" ] [ text value ]
                    | None ->
                        empty
            ]
        ]

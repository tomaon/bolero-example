namespace App.Client.Components

open Elmish
open Bolero
open Bolero.Html
open Bolero.Remoting


module CounterComponent =


    [<RequireQualifiedAccess>]
    type Msg =
        | Increment
        | Decrement
        | SetCounter of int


    type Model =
        { Value: int }


    let init (value: int) =
        { Value = value }, Cmd.none
    

    let update (remoteProvider: IRemoteProvider) (msg: Msg) (model: Model) =
        match msg with
        | Msg.Increment ->
            { model with Value = model.Value + 1 }, Cmd.none
        | Msg.Decrement ->
            { model with Value = model.Value - 1 }, Cmd.none
        | Msg.SetCounter value ->
            { model with Value = value }, Cmd.none


    let view (router: Router<_, _, _>) (model: Model) (dispatch: Dispatch<Msg>) =
        section [ "id" => "counter"; "class" => "section" ] [
            h1 [ "class" => "title" ] [ text "A simple counter" ]
            p [] [
                button [ "class" => "button"; on.click (fun _ -> dispatch Msg.Decrement) ] [ text "-" ]
                input [ "type" => "number"; "class" => "input"; bind.change.int model.Value (Msg.SetCounter >> dispatch) ]
                button [ "class" => "button"; on.click (fun _ -> dispatch Msg.Increment) ] [ text "+" ]
            ]
            p [] [ textf "The counter's value is: %d" model.Value ]
        ]

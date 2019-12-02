namespace App.Client

open Elmish
open Bolero
open Bolero.Html
open Bolero.Remoting
open Bolero.Remoting.Client

open App.Client.Components
open App.Client.Extensions
open App.Client.Services


module Index =


    [<RequireQualifiedAccess>]
    type Msg =
        | SetPage of Page
        | RecvHome of HomeComponent.Msg
        | SendHome of unit
        | RecvCounter of CounterComponent.Msg
        | SendCounter of int
        | RecvData of DataComponent.Msg
        | SendData of Book []
        | RecvSignIn of SignInComponent.Msg
        | SendSignIn
        | RecvSignOut of unit
        | SendSignOut
        | RecvGetUserName of string
        | SendGetUserName
        | RecvIgnore of exn
        | RecvError of exn
        | Error of string


    type Model =
        { Page: Page
          Home: HomeComponent.Model option
          Counter: CounterComponent.Model option
          Data: DataComponent.Model option
          SignIn: SignInComponent.Model option
          Name: string option
          Error: string option }


    let private router =
        Router.infer Msg.SetPage (fun model -> model.Page)


    let private init _ =
        { Page = Page.Home
          Home = None
          Counter = None
          Data = None
          SignIn = None
          Name = None
          Error = None }, Cmd.ofMsg (Msg.SendGetUserName)


    let private update (remoteProvider: IRemoteProvider) (msg: Msg) (model: Model) =
        match msg with

        | Msg.SetPage page ->
            let m =
                match model.Page with
                | Page.Home -> { model with Page = page; Home = None; SignIn = None }
                | Page.Counter -> { model with Page = page; Counter = None; SignIn = None }
                | Page.Data -> { model with Page = page; Data = None; SignIn = None }
            let c =
                match page with
                | Page.Home -> Cmd.ofMsg (Msg.SendHome ())
                | Page.Counter -> Cmd.ofMsg (Msg.SendCounter 0)
                | Page.Data -> Cmd.ofAsync remoteProvider.V1.GetBooks () Msg.SendData Msg.RecvError
            m, c

        | Msg.RecvSignIn inner ->
            match model.SignIn with
            | Some value ->
                match inner with
                | SignInComponent.Msg.Ok name ->
                    { model with Name = Some name }, Cmd.ofMsg (Msg.SetPage model.Page)
                | _ ->
                    let signIn, cmd = SignInComponent.update remoteProvider inner value
                    { model with SignIn = Some signIn }, Cmd.map Msg.RecvSignIn cmd
            | None ->
                model, Cmd.none
        | Msg.SendSignIn ->
            let signIn, cmd = SignInComponent.init ()
            { model with SignIn = Some signIn }, Cmd.map Msg.RecvSignIn cmd
        | Msg.RecvSignOut () ->
            { model with Name = None }, Cmd.ofMsg (Msg.SetPage Page.Home)
        | Msg.SendSignOut ->
            model, Cmd.ofAsync remoteProvider.V1.SignOut () Msg.RecvSignOut Msg.RecvIgnore
        | Msg.RecvGetUserName name ->
            { model with Name = Some name }, Cmd.none
        | Msg.SendGetUserName ->
            model, Cmd.ofAsync remoteProvider.V1.GetUsername () Msg.RecvGetUserName Msg.RecvIgnore

        | Msg.RecvIgnore _ ->
            model, Cmd.none
        | Msg.RecvError e ->
            if e = RemoteUnauthorizedException
            then model, Cmd.ofMsg (Msg.SendSignIn)
            else model, Cmd.ofMsg (Msg.Error e.Message)

        | Msg.Error message ->
            { model with Error = Some message }, Cmd.none

        | _ ->
            match model.Page with

            | Page.Home ->
                match msg with
                | Msg.RecvHome inner ->
                    match model.Home with
                    | Some value ->
                        let home, cmd = HomeComponent.update remoteProvider inner value
                        { model with Home = Some home }, Cmd.map Msg.RecvHome cmd
                    | None ->
                        model, Cmd.none
                | Msg.SendHome args ->
                    let home, cmd = HomeComponent.init args
                    { model with Home = Some home }, Cmd.map Msg.RecvHome cmd
                | _ ->
                    model, Cmd.none

            | Page.Counter ->
                match msg with
                | Msg.RecvCounter inner ->
                    match model.Counter with
                    | Some value ->
                        let counter, cmd = CounterComponent.update remoteProvider inner value
                        { model with Counter = Some counter }, Cmd.map Msg.RecvCounter cmd
                    | None ->
                        model, Cmd.none
                | Msg.SendCounter args ->
                    let counter, cmd = CounterComponent.init args
                    { model with Counter = Some counter }, Cmd.map Msg.RecvCounter cmd
                | _ ->
                    model, Cmd.none

            | Page.Data ->
                match msg with
                | Msg.RecvData inner ->
                    match model.Data with
                    | Some value ->
                        let data, cmd = DataComponent.update remoteProvider inner value
                        { model with Data = Some data }, Cmd.map Msg.RecvData cmd
                    | None ->
                        model, Cmd.none
                | Msg.SendData args ->
                    let data, cmd = DataComponent.init args
                    { model with Data = Some data }, Cmd.map Msg.RecvData cmd
                | _ ->
                    model, Cmd.none


    let private menuItem model page str =
        if model.Page = page then
            li [] [ a [ "class" => "is-active"; "style" => "cursor:default;" ] [ text str ] ]
        else
            li [] [ a [ router.HRef page ] [ text str ] ]

    let private spinner =
        div [] [ text "Loading..." ] // TODO


    let private view (model: Model) (dispatch: Dispatch<Msg>) =
        concat [
            nav [ "class" => "navbar is-dark" ] [
                div [ "class" => "navbar-brand" ] [
                    a [ "class" => "navbar-item has-text-weight-bold is-size-5"; "href" => "https://fsbolero.io" ] [ text "Bolero" ]
                ]
                cond model.Name <| function
                    | Some value ->
                        div [ "class" => "navbar-end" ] [
                            div [ "class" => "navbar-item" ] [ textf "Signed in as %s." value ]
                            div [ "class" => "navbar-item" ] [
                                div [ "class" => "buttons" ] [
                                    button [ "class" => "button is-dark is-inverted is-outlined"; on.click (fun _ -> dispatch Msg.SendSignOut) ] [ text "Sign out" ]
                                ]
                            ]
                        ]
                    | None ->
                        empty
            ]
            div [ "class" => "columns" ] [
                aside [ "class" => "column sidebar is-narrow" ] [
                    section [ "class" => "section" ] [
                        nav [ "class"=> "menu" ] [
                            ul [ "class" => "menu-list" ] [
                                menuItem model Page.Home "Home"
                                menuItem model Page.Counter "Counter"
                                menuItem model Page.Data "Download data"
                            ]
                        ]
                    ]
                ]
                main [ "class" => "column" ] [
                    cond model.SignIn <| function
                        | Some value ->
                            SignInComponent.view router value (Msg.RecvSignIn >> dispatch)
                        | None ->
                            match model.Page with
                            | Page.Home ->
                                match model.Home with
                                | Some value ->
                                    HomeComponent.view router value (Msg.RecvHome >> dispatch)
                                | None ->
                                    spinner
                            | Page.Counter ->
                                match model.Counter with
                                | Some value ->
                                    CounterComponent.view router value (Msg.RecvCounter >> dispatch)
                                | None ->
                                    spinner
                            | Page.Data ->
                                match model.Data with
                                | Some value ->
                                    DataComponent.view router value (Msg.RecvData >> dispatch)
                                | None ->
                                    spinner
                    cond model.Error <| function
                        | Some value ->
                            div [ "class" => "notification is-warning" ] [ text value ]
                        | None ->
                            empty
                ]
            ]
        ]


    type Main() =
        inherit ProgramComponent<Model, Msg>()

        override self.Program =
            Program.mkProgram init (update (self.RemoteProvider())) view
            |> Program.withRouter router
#if DEBUG
            |> Program.withConsoleTrace
#endif

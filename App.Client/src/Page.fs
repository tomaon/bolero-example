namespace App.Client

open Bolero


[<RequireQualifiedAccess>]
type Page =
    | [<EndPoint "/">] Home
    | [<EndPoint "/counter">] Counter
    | [<EndPoint "/data">] Data

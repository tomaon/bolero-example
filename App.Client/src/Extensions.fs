namespace App.Client

open Bolero.Remoting

open App.Client.Services


module Extensions =


    type IRemoteProvider with

        member self.V1 = self.GetService<V1Service>()

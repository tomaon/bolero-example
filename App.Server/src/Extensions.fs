namespace App.Server

open Microsoft.EntityFrameworkCore

open Bolero.Remoting.Server

open App.Client.Services


module Extensions =


    type IRemoteContext with

        member inline self.GetServiceProvider () =
             self.HttpContext.RequestServices

        member inline self.GetDbContext<'T> () =
            self.GetServiceProvider().GetService typeof<'T> :?> 'T


    type Book with

        static member FindAllAsNoTracking (dbContext: AppDbContext) =
            query {
                for book in dbContext.Book.AsNoTracking() do
                sortBy book.Title
                select book
            }

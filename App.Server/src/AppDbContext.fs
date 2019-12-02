namespace App.Server

open Microsoft.EntityFrameworkCore

open App.Client.Services


type AppDbContext =
    inherit DbContext

    new() = { inherit DbContext() }
    new(options: DbContextOptions<AppDbContext>) = { inherit DbContext(options) }

    override __.Dispose() = ()


    [<DefaultValue>]
    val mutable private book: DbSet<Book>

    member self.Book
        with get () = self.book
        and set book = self.book <- book
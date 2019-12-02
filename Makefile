#
default: build run

#
build:
	dotnet $@

init:
	sqlite3 App.Server/Data/Development.sq3 < App.Server/Data/default.sql

run:
	dotnet run -p ./App.Server

sln:
	dotnet new sln --force
	dotnet sln add ./App.Client ./App.Server

clean:
	rd /s /q .\.ionide
	rd /s /q .\App.Client\bin
	rd /s /q .\App.Client\obj
	rd /s /q .\App.Server\bin
	rd /s /q .\App.Server\obj

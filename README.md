# Redgum Server Monitor

Nifty little monitoring website with a powershell-based client

Originally developed as an in-house tool by Redgum Technologies

Developed in Visual Studio 2017 (VSCode would work nicely too)

## Installing

This uses a local database.. change the connection string in the config to change the name of the db that is created

There are two db's in use (mooshed into the one actual db)

The security is handled magically, the best way to make that work is to run the index page and it will guide you through creating the db

Until we work out how to do the same thing, the actual db for storing the monitoring information will need to be created manually

Open a command prompt in the web directory and run
``` dotnet ef database update --context MonitorDbContext ```

## Entity Framework Core Stuff

TODO 
.. work out how the hell to update the db in prod

### Changing a schema

Info from here...
https://docs.microsoft.com/en-us/aspnet/core/data/ef-mvc/migrations

Open a command prompt in the web directory

* change "InitialCreate" to the name of your migration
* "MonitorDbContext" is the name of the context .. change this if you're migrating a different context

``` dotnet ef migrations add InitialCreate --context MonitorDbContext ```

Then to actually run the migration on the db

``` dotnet ef database update --context MonitorDbContext ```


### More EF Core info

https://docs.microsoft.com/en-us/aspnet/core/data/ef-mvc/intro

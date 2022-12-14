# Maintenance buddy

> Manage maintenances and get reminders about pending tasks. 
The history and notes of maintenances will simplify maintenances in the future and keep the machines running ;)

## Motivation

I own some bicycles and motorcycles which I maintain. In the past I used some excel sheets about the maintenances. But I didn't saw what maintenances are pending. Therefore I started to create an application that helps me. In future the app shouldn't be limited to vehicles.


## Features and Roadmap

*This project is still in development*

The app has a [web frontend](https://maintenance-buddy.georg-braun.de) that connects to a backend. The backend persists the data in a database and run some calculations on the provided data (like peneding maintenances) and supply these information via some endpoints.


### ✔️ Manage vehicles and their maintenances
You can add vehicles and add necessary maintenances templates. A template is a recurring maintenance like a oil exchange every 5000 km or every year.
Add a finished maintenance that corresponds to a previously created maintenance template.

### ✔️ See pending maintenances
You get a list of pending maintenances or maintenances that will be relevant in the near future

### Get a notification of pending changes
The idea is to get a notification via mail

## Technologies, techniques and principles

- Backend (API)
  - ASP.NET Core
  - Entity Framework Core
  - data persistence with a PostgreSQL database
  - API is designed in CQRS style instead of REST
  - Model domain with domain-driven design approach
  - deployment in a docker container on a digital ocean vps
  - Integration tests that cover the whole API (I like to develop new API endpoints in a TDD manner)
  - Continuous integration on every github commit 
  - Continuous deployment of the container image on every PR merge to the release branch
- Frontend
  - Svelte(Kit)
  - deployed via vercel with every commit to the release branch
- Authentication (Frontend + API)
  - Auth0


General development principles:
- YAGNI - You aren't gonna need it: Especially the backend has some potential technial debts (e.g. domain objects as persistence objects). But as long as it's not necessary I won't refactor this. But it's important to be aware of these things.


# How can I setup this on my computer?

To get everything running you need an authentication provider (e.g. Auth0), a postgresql database and you have to configure some things in the back- and frontend

## [1/4] Setup an authentication provider

I use Auth0 for my instances. You have to add a frontend and backend application.


## [2/4] Setup database

You have to setup a postgres database server. Most of the time I use a postgres docker container.

In this repository you can find a cli tool that will help to migrate the database.

```bash
./utility-cli.exe migrate "Host=localhost;Port=5432;Database=maintenance;Username=postgres;Password=postgres" "C:\Development\Repositories\maintenance-buddy\maintenance-buddy-api\maintenance-buddy-api\Migrations"
```

## [3/4] Setup backend

You have to add authentication parameters (e.g. Auth0) and the connection string to the PostgreSQL database.

One possiblity is to modify the appsettings.json
```json
  "DbConnectionString": "Host=localhost;Port=5432;Database=maintenance;Username=postgres;Password=postgres",
  "AuthProvider": {
    "Authority": "",
    "Audience": ""
```

But I recommend to use the dotnet secrets manager.

## [4/4] Setup frontend

To use the correct authentication provider and to talk to the correct backend you have to set some environment variables.

One possibility is to create a `.env` file and add the necessary environment varialbes with a `PUBLIC_` prefix
```
PUBLIC_AUTH_DOMAIN=
PUBLIC_AUTH_CLIENT_ID=
PUBLIC_AUTH_AUDIENCE=
PUBLIC_API_SERVER_ADDRESS=
```

After this you can
```
npm install
npm run dev
```

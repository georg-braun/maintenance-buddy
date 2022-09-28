# Maintenance buddy

> Manage maintenances and get reminders about pending tasks. 
The history and notes of maintenances will simplify maintenances in the future and keep the machines running ;)

## Motivation

I own some bicycles and motorcycles which I maintain. In the past I used some excel sheets about the done maintenances. But I didn't saw what maintenances are pending. Therefore I started to create an application that helps me. In future the app shouldn't be limited to vehicles.


## Features and Roadmap

*This project is still in development*

The app has a [web frontend](https://maintenance-buddy.georg-braun.de) that connects to a backend. The backend persists the data in a database and run some calculations on the provided data (like peneding maintenances) and supply these information via some endpoints.


### ✔️ Manage vehicles and their action templates 
You can add vehicles and add necessary maintenances templates. A template is a recurring maintenance like a oil exchange every 5000 km or every year.

### ✔️ Manage finished maintenances
Add a finished maintenance that corresponds to a previously created maintenance template.

### See pending maintenances
You get a list of pending maintenances or maintenances that will be relevant in the near future

### Get a notification of pending changes
The idea is to get a notification via mail

## Technologies and techniques

Used technologies:
- Backend (API)
  - ASP.NET Core
  - Entity Framework Core
  - data persistence with a PostgreSQL database
  - deployment in a docker container on a digital ocean vps
  - Integration tests that cover the whole API (I like to develop new API endpoints in a TDD manner)
  - Continuous integration on every github commit 
- Frontend
  - Svelte(Kit)
  - deployed via vercel
- Authentication (Frontend + API)
  - Auth0


Used techniques:
- CQRS backend API design instead of REST
- YAGNI - You aren't gonna need it: Especially the backend has some potential technial debts (e.g. domain objects as persistence objects). But as long as it's not necessary I won't refactor this. But it's important to be aware of these things.


# Local dev setup

## Webapp

### Add authentication

- create `.env` and add the necessary environment varialbes with a `PUBLIC_` prefix
```
PUBLIC_AUTH_DOMAIN=
PUBLIC_AUTH_CLIENT_ID=
PUBLIC_AUTH_AUDIENCE=
```

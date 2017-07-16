# Apollo

This is my personal artificial brain and chat bot.

Locally, the server is meant to run in the vagrant box, but the client can run
from either the vm or locally.  The performance of the compiler is much better
locally and is run more often, so this is my preference.

## Running

Currently I can only guarantee this runs on Ubuntu 16.04.

To setup client: change to the client directory and run `yarn install`.

`yarn start` will start the development react client.

The server needs to be run from the vagrant box:

```
$ vagrant up
$ vagrant ssh
$ cd /vagrant
$ alembic upgrade head
$ cd server
$ ./build
$ ./server

```

Once the server is running, you ought to be able to use the `./console` command in
that directory to communicate with the server, and the React client ought to work.

I'll be working to make this more automated in the future.

The default password is `wokka`.

## Deploying

Well, it's not really ready to be deployed except in safe environments.  If you really
want to deploy it out, update the deploy.yml file with the appropriate variable values
and the update the inventory to reflect the ip address of the box you're deploying to.

I'm assuming that you have ssh access setup to the target box at the moment.

## Tech
### Server: .NET/C# on Dotnet Core

The server was originally written against Mono, but dotnet core finally matured enough
that I ported it over, and it seems to be more stable.

Kestrel is used as a dumb webserver and passes off the requests to my JSONRPC
processor.  The code around this is a little awkward because it was originally
written against `HttpListener` directly, but Kestrel is better.  Note: I am
not using REST, but JSONRPC.  That is a very important difference.

### Client: React

I'm still very new to React, so things are very simple.  I am also using
axion for ajax requests, bootstrap, moment, and store.

I may end up converting this to ReactNative eventually, because I like the idea
of being able to access this service via an app on my phone.

### Database: Postgres

Migrations are handled with Alembic.

### Infrastructure: Ansible

`i <3 playbooks`

The roles are broken up because the development vagrant box is also provisioned
using those same roles.

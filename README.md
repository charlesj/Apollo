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

## Tech
### Server: .NET/C# on Mono

I wanted a simple JSON RPC interface that could be used to invoke commands on 
server.  This was done by using the `HttpListener` class.  I had to introduce
a few facades to make it more testable.  

Libraries: xUnit, Moq, Json.Net, Dapper (Async), SimpleInjector, Npgsql

There is also a command line client written using the CommandLineParser 
library.  The integration tests all use this client, so there is currently 
some handholding that needs to happen in order to make this nicer.

- Client: React

I'm still very new to React, so things are very simple.  I am also using 
axion for ajax requests, bootstrap, moment, and store.

I may end up converting this to ReactNative eventually, because I like the idea
of being able to access this service via an app on my phone.

- Database: Postgres

Migrations are handled with Alembic.

- Infrastructure: Ansible 

`i <3 playbooks`

The roles are broken up because the development vagrant box is also provisioned
using those same roles.


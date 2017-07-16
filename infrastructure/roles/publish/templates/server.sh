#!/bin/bash
set -e

APOLLO_EXE=/var/apollo/server/Apollo.dll

# database configuration
APOLLO_DATABASENAME='{{ dbname }}'
APOLLO_DATABASESERVER='127.0.0.1'
APOLLO_DATABASEUSERNAME='{{ dbuser }}'
APOLLO_DATABASEPASSWORD='{{ dbpassword }}'

export APOLLO_DATABASENAME
export APOLLO_DATABASESERVER
export APOLLO_DATABASEUSERNAME
export APOLLO_DATABASEPASSWORD

dotnet "$APOLLO_EXE"

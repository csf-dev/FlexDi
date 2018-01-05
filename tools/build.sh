#!/bin/bash

cd "$(dirname "$0")/.."

function exit_if_failed()
{
  if [ "$1" -ne "0" ]
  then
    echo "The build step '$2' failed"
    exit 1
  fi
}

./tools/install.sh

msbuild
exit_if_failed $? "msbuild"

./tools/run-tests.sh
exit_if_failed $? "Unit & integration tests"

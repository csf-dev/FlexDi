#!/bin/bash

cd "$(dirname "$0")/.."

./tools/install.sh
msbuild
./tools/run-tests.sh

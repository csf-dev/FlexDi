#!/bin/bash

cd "$(dirname "$0")/.."

nuget restore CSF.MicroDi.sln
nuget install NUnit.ConsoleRunner -Version 3.6.1 -OutputDirectory testrunner
nuget install NUnit.Extension.NUnitV2Driver -Version 3.6.0 -OutputDirectory testrunner
cp testrunner/NUnit.Extension.NUnitV2Driver.3.6.0/tools/* testrunner/NUnit.ConsoleRunner.3.6.1/tools/

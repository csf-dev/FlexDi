#!/bin/bash

cd "$(dirname "$0")/.."

nuget restore CSF.FlexDi.sln
nuget install NUnit.ConsoleRunner -Version 3.6.1 -OutputDirectory testrunner
nuget install NUnit.Extension.NUnitV2Driver -Version 3.6.0 -OutputDirectory testrunner

#!/bin/bash

cd "$(dirname "$0")/.."

TEST_RUNNER="testrunner/NUnit.ConsoleRunner.3.6.1/tools/nunit3-console.exe"

mono "$TEST_RUNNER" \
  CSF.MicroDi.Tests/bin/Debug/CSF.MicroDi.Tests.dll \
  CSF.MicroDi.BoDiCompatibility.Tests/bin/Debug/CSF.MicroDi.BoDiCompatibility.Tests.dll \
  CSF.MicroDi.BoDiCompatibility.IntegrationTests/bin/Debug/BoDi.Tests.dll
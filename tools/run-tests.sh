#!/bin/bash

cd "$(dirname "$0")/.."

TEST_RUNNER="testrunner/NUnit.ConsoleRunner.3.6.1/tools/nunit3-console.exe"

mono "$TEST_RUNNER" \
  CSF.FlexDi.Tests/bin/Debug/CSF.FlexDi.Tests.dll \
  CSF.FlexDi.BoDiCompatibility.Tests/bin/Debug/CSF.FlexDi.BoDiCompatibility.Tests.dll \
  CSF.FlexDi.BoDiCompatibility.IntegrationTests/bin/Debug/BoDi.Tests.dll

exit $?
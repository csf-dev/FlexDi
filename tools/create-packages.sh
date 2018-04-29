#!/bin/bash

msbuild /p:Configuration=Release

if [ "$?" -ne "0" ]
then
  exit 1
fi

sn -R CSF.FlexDi/bin/Release/CSF.FlexDi.dll CSF-Software-OSS.snk
sn -R CSF.FlexDi.BoDiCompatibility/bin/Release/BoDi.dll CSF-Software-OSS.snk

find . \
  -type f \
  -name "*.nuspec" \
  \! -path "./.git/*" \
  -exec nuget pack '{}' \;
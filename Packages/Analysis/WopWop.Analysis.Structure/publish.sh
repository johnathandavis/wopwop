#!/bin/bash
set -euio pipefail
dotnet clean
dotnet build -c Release
dotnet pack
PKG=$(ls bin/Release/WopWop.Analysis.Structure*)
echo "Pushing package $PKG..."
nuget push $PKG -Source https://api.nuget.org/v3/index.json


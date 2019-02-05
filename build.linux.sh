#!/bin/bash

dotnet publish --self-contained -c Release -r linux-x64 -o ../build/linux-x64/Speedtest Speedtest/Speedtest.csproj
dotnet publish --self-contained -c Release -r linux-x64 -o ../build/linux-x64/Speedtest.InfluxDb Speedtest.InfluxDb/Speedtest.InfluxDb.csproj
dotnet build -c Release -r linux-x64 -o ../build/linux-x64-thin/Speedtest Speedtest/Speedtest.csproj
dotnet build -c Release -r linux-x64 -o ../build/linux-x64-thin/Speedtest.InfluxDb Speedtest.InfluxDb/Speedtest.InfluxDb.csproj

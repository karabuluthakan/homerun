﻿FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["notification/src/Consumer/Consumer.csproj", "notification/src/Consumer/"]
COPY ["shared/Common/Common.csproj", "shared/Common/Common/"]
COPY ["shared/Contract/Contract.csproj", "shared/Contract/Contract/"]
COPY ["shared/Core/Core.csproj", "shared/Core/Core/"]
RUN dotnet restore "notification/src/Consumer/Consumer.csproj"
COPY . .
WORKDIR "/src/notification/src/Consumer"
RUN dotnet build "Consumer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Consumer.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Consumer.dll"]

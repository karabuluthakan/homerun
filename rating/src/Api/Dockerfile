FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["rating/src/Api/Api.csproj", "rating/src/Api/"]
COPY ["rating/src/Domain/Domain.csproj", "rating/src/Domain/"]
COPY ["rating/src/Infrastructure/Infrastructure.csproj", "rating/src/Infrastructure/"]
RUN dotnet restore "rating/src/Api/Api.csproj"
COPY . .
WORKDIR "/src/rating/src/Api"
RUN dotnet build "Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Api.dll"]

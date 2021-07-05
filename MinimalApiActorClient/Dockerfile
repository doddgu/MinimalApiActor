#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MinimalApiActorClient/MinimalApiActorClient.csproj", "MinimalApiActorClient/"]
COPY ["MinimalApiActor/MinimalApiActor.csproj", "MinimalApiActor/"]
RUN dotnet restore "MinimalApiActorClient/MinimalApiActorClient.csproj"
COPY . .
WORKDIR "/src/MinimalApiActorClient"
RUN dotnet build "MinimalApiActorClient.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MinimalApiActorClient.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MinimalApiActorClient.dll"]
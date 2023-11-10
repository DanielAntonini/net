FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Examples.Api/Examples.Api.csproj", "Examples.Api/"]
RUN dotnet restore "Examples.Api/Examples.Api.csproj"
COPY . .
WORKDIR "/src/Examples.Api"
RUN dotnet build "Examples.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Examples.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Examples.Api.dll"]

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 8000
ENV ASPNETCORE_ENVIRONMENT=Docker

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY "nuget.config" "FLS.CoffeeDesk/nuget.config"
COPY ["FLS.CoffeeDesk/FLS.CoffeeDesk.csproj", "FLS.CoffeeDesk/"]
RUN dotnet restore "FLS.CoffeeDesk/FLS.CoffeeDesk.csproj"
COPY . .
WORKDIR "/src/FLS.CoffeeDesk"
RUN dotnet build "FLS.CoffeeDesk.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FLS.CoffeeDesk.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FLS.CoffeeDesk.dll"]

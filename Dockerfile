FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["DRPGServer.csproj", "./"]
RUN dotnet restore "DRPGServer.csproj"
COPY . .

RUN dotnet publish "DRPGServer.csproj" -c Release -o /app/publish
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:80
COPY --from=build /app/publish .
EXPOSE 80

ENTRYPOINT ["dotnet", "DRPGServer.dll"]
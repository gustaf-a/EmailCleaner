
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5202

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MailProviderService/MailProviderService.csproj", "MailProviderService/"]
RUN dotnet restore "MailProviderService/MailProviderService.csproj"
COPY . .
WORKDIR "/src/MailProviderService"
RUN dotnet build "MailProviderService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MailProviderService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MailProviderService.dll"]
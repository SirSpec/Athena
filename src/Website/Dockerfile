ARG NET_VERSION

FROM mcr.microsoft.com/dotnet/aspnet:${NET_VERSION} AS base
WORKDIR /app
EXPOSE 80
ENV ASPNETCORE_URLS=http://+:80

FROM mcr.microsoft.com/dotnet/sdk:${NET_VERSION} AS publish
ARG CONFIGURATION
WORKDIR /src
COPY . .
RUN dotnet restore
WORKDIR /src/src/Website
RUN dotnet publish "Website.csproj" --configuration ${CONFIGURATION} --output /app /p:UseAppHost=false

FROM base AS final
ARG ASPNETCORE_ENVIRONMENT
WORKDIR /app
COPY --from=publish /app .
ENV ASPNETCORE_ENVIRONMENT=${ASPNETCORE_ENVIRONMENT}
ENTRYPOINT ["dotnet", "Website.dll"]

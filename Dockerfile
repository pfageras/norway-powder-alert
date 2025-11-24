# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY src/NorwayPowderAlert/NorwayPowderAlert.csproj ./NorwayPowderAlert/
RUN dotnet restore ./NorwayPowderAlert/NorwayPowderAlert.csproj

# Copy everything else and build
COPY src/NorwayPowderAlert/. ./NorwayPowderAlert/
WORKDIR /src/NorwayPowderAlert
RUN dotnet build -c Release -o /app/build
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "NorwayPowderAlert.dll"]

# stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

# Copy only project files
COPY ["WebAPI/WebAPI.csproj", "WebAPI/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["Domain/Domain.csproj", "Domain/"]

# Restore dependencies (cached unless .csproj changes)
RUN dotnet restore "WebAPI/WebAPI.csproj"

# Copy the remaining source code
COPY . .

# Publish the application
# --configuration Release: Build using the Release configuration
# --output /app/publish: Store the published application files in /app/publish
# --no-restore: Skip restore because dependencies were already restored
RUN dotnet publish "WebAPI/WebAPI.csproj" --configuration Release --output /app/publish --no-restore



# stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime

WORKDIR /app

COPY --from=build /app/publish .

# Configure ASP.NET Core to listen on port 8080
ENV ASPNETCORE_URLS=http://+:8080

# Document the port used by the ASP.NET Core application
EXPOSE 8080

# Start the ASP.NET Core application
ENTRYPOINT ["dotnet", "WebAPI.dll"]
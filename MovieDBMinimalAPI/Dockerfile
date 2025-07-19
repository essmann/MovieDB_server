# Stage 1: Build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies (cache this step)
COPY ["MovieDBMinimalAPI.csproj", "./"]
RUN dotnet restore "./MovieDBMinimalAPI.csproj"

# Copy the rest of the source code
COPY . .

# Build and publish the app to /app/publish folder
RUN dotnet publish "./MovieDBMinimalAPI.csproj" -c Release -o /app/publish

# Stage 2: Create runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Expose port 80 for the container
EXPOSE 80

# Run the app
ENTRYPOINT ["dotnet", "MovieDBMinimalAPI.dll"]

# Use .NET 8 SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the .csproj and restore dependencies
COPY ["GameSubmission.csproj", "GameSubMicroService/"]
WORKDIR "/src/GameSubMicroService"
RUN dotnet restore "GameSubmission.csproj"

# Copy everything and build
COPY . .
RUN dotnet build "GameSubmission.csproj" -c Release -o /app/build

# Publish the project
RUN dotnet publish "GameSubmission.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Use the ASP.NET 8 runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Expose port (optional, but recommended)
EXPOSE 4001

# Run the application
ENTRYPOINT ["dotnet", "GameSubmission.dll"]

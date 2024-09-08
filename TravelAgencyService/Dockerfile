# Use the official Microsoft .NET Core runtime base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

# Set the working directory
WORKDIR /app

# Expose the port your app runs on
EXPOSE 5102

# Set the ASPNETCORE_HTTP_PORTS environment variable
ENV ASPNETCORE_HTTP_PORTS=5102

# Use the official Microsoft .NET Core SDK image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the build configuration
ARG BUILD_CONFIGURATION=Release

# Set the working directory
WORKDIR /src

# Copy the project file
COPY ["TravelAgencyService.csproj", "./"]

# Restore the project
RUN dotnet restore "TravelAgencyService.csproj"

# Copy the rest of the files
COPY . .

# Copy the HTML files
COPY ./AdditionalFiles /app/AdditionalFiles

# Set the working directory
WORKDIR "/src/"

# Build the project
RUN dotnet build "TravelAgencyService.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Use the build image to publish the project
FROM build AS publish

# Set the build configuration
ARG BUILD_CONFIGURATION=Release

# Publish the project
RUN dotnet publish "TravelAgencyService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Use the base image for the final image
FROM base AS final

# Set the working directory
WORKDIR /app

# Copy the published files
COPY --from=publish /app/publish .

# Copy the HTML files
COPY --from=build /app/AdditionalFiles ./AdditionalFiles

# Set the entrypoint with delay
ENTRYPOINT ["sh", "-c", "dotnet TravelAgencyService.dll"]
# Use the official Microsoft SQL Server 2022 image from the Docker Hub
FROM mcr.microsoft.com/mssql/server:2022-latest

# Set the environment variables
ENV ACCEPT_EULA=Y
ENV SA_PASSWORD=Test1234

# Expose the SQL Server port
EXPOSE 1433

# Start SQL Server
CMD /opt/mssql/bin/sqlservr

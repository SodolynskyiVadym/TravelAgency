# Use the official Microsoft SQL Server 2022 image from the Docker Hub
FROM mcr.microsoft.com/mssql/server:2022-latest

# Set the environment variables
ENV ACCEPT_EULA=Y
ENV SA_PASSWORD=Test1234

# Copy the database initialization script into the image
COPY init.sql /init.sql

# Expose the SQL Server port
EXPOSE 1433

# Start SQL Server and run the initialization script after a delay to ensure SQL Server is ready
CMD /bin/bash -c "/opt/mssql/bin/sqlservr & sleep 30 && /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P 'Test1234' -d master -i /init.sql && wait"

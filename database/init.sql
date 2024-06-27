IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'travel_agency')
BEGIN
    CREATE DATABASE travel_agency;
    PRINT 'Database created';
END
ELSE 
BEGIN
    PRINT 'Database already exists';
END
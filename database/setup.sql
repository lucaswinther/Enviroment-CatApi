

IF NOT Exists(
    SELECT Name
    FROM sys.databases
    WHERE name = 'DBCatApi' 
)
BEGIN
    CREATE DATABASE DBCatApi
END
GO

USE DBCatApi
GO
